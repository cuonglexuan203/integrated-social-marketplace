import weaviate, { vectorizer, dataType } from 'weaviate-client';
import axios from 'axios';
import dotenv from 'dotenv';
import express from 'express';
import ollama from 'ollama';

dotenv.config();

class RecommendationService {
    constructor() {

        // Interaction scores
        this.interactionScores = {
            like: 2,
            love: 3,
            sad: 2,
            angry: 1,
            comment: 4,
            share: 5,
        };
    }

    async connectWeaviate() {
        // Weaviate Client
        this.weaviateClient = await weaviate.connectToLocal({
            host: process.env.WEAVIATE_HOST || '127.0.0.1',
            port: process.env.WEAVIATE_PORT || "11434"
        });
    }

    /**
     * Initialize Weaviate schema
     */
    async initializeWeaviateSchema() {

        const schema = {
            name: 'Post',
            vectorizers: vectorizer.text2VecOllama({
                apiEndpoint: 'http://localhost:11434',
                model: 'nomic-embed-text',
            }),
            properties: [
                { name: 'postId', dataType: dataType.TEXT },
                { name: 'contentText', dataType: dataType.TEXT },
                { name: 'tags', dataType: dataType.TEXT_ARRAY },
                { name: 'category', dataType: dataType.TEXT }
            ]
        };

        try {
            if (!await this.weaviateClient.collections.exists("Post"))
                await this.weaviateClient.collections.create(schema);
        } catch (error) {
            console.error('Error creating Weaviate schema:', error);
        }
    }

    initCollections() {
        this.posts = this.weaviateClient.collections.get("Post");
    }

    /**
     * Generate embedding using nomic-embed-text
     */
    async generateEmbedding(text) {
        try {
            const response = await ollama.embeddings({
                model: 'nomic-embed-text',
                prompt: text,
            });
            return response.embedding;
        } catch (error) {
            console.error('Error generating embedding:', error);
            return null;
        }
    }

    /**
     * Add or update post in Weaviate
     */
    async upsertPostVector(post) {
        try {
            // Adjust weights for description, tags, and category
            const contentTextWeight = 3;
            const tagsWeight = 2;
            const categoryWeight = 1;

            // Repeat strings to apply weights
            const weightedContentText = post.contentText ? Array(contentTextWeight).fill(post.contentText).join(' ') : '';
            const weightedTags = post.tags ? Array(tagsWeight).fill(post.tags.join(' ')).join(' ') : '';
            const weightedCategory = post.category ? Array(categoryWeight).fill(post.category).join(' ') : '';

            // Combine weighted components
            const text = `${weightedContentText} ${weightedTags} ${weightedCategory}`.trim();
            // Generate vector embedding
            const vector = await this.generateEmbedding(text);
            if (!vector) return;

            // Upsert post vector to database
            await this.posts.data.insert({
                metadata: post.id,
                properties: {
                    contenText: post.contentText,
                    tags: post.tags,
                    category: post.category,
                    postId: post.id
                },
                // id: post.id,
                vectors: vector,

            });
        } catch (error) {
            console.error('Error upserting post vector:', error);
        }
    }


    /**
     * Delete post vector from Weaviate
     */
    async deletePostVector(postId) {
        try {
            await this.posts.data.deleteById(postId);
        } catch (error) {
            console.error('Error deleting post vector:', error);
        }
    }

    /**
     * Load initial posts into Weaviate
     */
    async loadInitialPosts() {
        try {
            if (await this.posts.length()) return;
            const response = await axios.get(`${process.env.FEED_SERVICE}/api/v1/Post/GetPosts`);
            const posts = response.data.result?.data;
            // console.log(posts.result.data)
            for (const post of posts) {
                await this.upsertPostVector(post);
            }
        } catch (error) {
            console.error('Error loading initial posts:', error);
        }
    }

    /**
     * Get top user interactions with scores
     */
    async getTopUserInteractions(userId) {
        try {
            const response = await axios.get(
                `${process.env.FEED_SERVICE}/api/v1/interactions/GetUserInteractions?userId=${userId}`
            );

            const interactions = response.data
                .map(interaction => ({
                    ...interaction,
                    score: this.interactionScores[interaction.interactionType] || 0
                }))
                .sort((a, b) => b.timestamp - a.timestamp)
                .slice(0, 10);

            return interactions;
        } catch (error) {
            console.error('Error in getTopUserInteractions:', error);
            return [];
        }
    }

    /**
     * Get relevant posts for a user
     */
    async getRelevantPosts(userId, limit = 20) {
        try {
            // Get top interactions
            const topInteractions = await this.getTopUserInteractions(userId);
            if (!topInteractions.length) {
                return this.getFallbackRecommendations(limit);
            }

            // Get vectors for interactions
            const vectors = [];
            for (const interaction of topInteractions) {
                const postVector = await this.posts.query.fetchObjectById(interaction.postId.toString());

                if (postVector?.vector) {
                    // Weight vector by interaction score
                    const weightedVector = postVector.vector.map(v => v * interaction.score);
                    vectors.push(weightedVector);
                }
            }

            if (!vectors.length) {
                return this.getFallbackRecommendations(limit);
            }

            // Calculate average vector
            const averageVector = this.calculateAverageVector(vectors);

            // Get similar posts using Weaviate
            // const result = await this.weaviateClient.graphql.get()
            //     .withClassName('Post')
            //     .withFields(['postId'])
            //     .withNearVector({
            //         vector: averageVector,
            //         distance: 0.8
            //     })
            //     .withLimit(limit)
            //     .do();

            const result = await this.posts.query.nearVector(averageVector, {
                limit: limit,
                // distance: 0.8
            });

            // Get full post details
            const postIds = result.data.Get.Post.map(post => post.postId);
            const posts = await axios.get(
                `${process.env.FEED_SERVICE}/api/v1/Post/GetPosts?ids=${postIds.join(',')}`
            );

            return posts.data;
        } catch (error) {
            console.error('Error in getRelevantPosts:', error);
            return this.getFallbackRecommendations(limit);
        }
    }

    /**
     * Calculate average vector
     */
    calculateAverageVector(vectors) {
        if (!vectors.length) return [];

        const vectorLength = vectors[0].length;
        const sumVector = new Array(vectorLength).fill(0);

        vectors.forEach(vector => {
            vector.forEach((value, index) => {
                sumVector[index] += value;
            });
        });

        return sumVector.map(sum => sum / vectors.length);
    }

    /**
     * Get fallback recommendations
     */
    async getFallbackRecommendations(limit) {
        try {
            const response = await axios.get(
                `HTTP://feed.api:9000/api/v1/Post/GetPosts?limit=${limit}`
            );
            return response.data;
        } catch (error) {
            console.error('Error in getFallbackRecommendations:', error);
            return [];
        }
    }

    async close() {
        await this.mongoClient.close();
    }
}

async function analyzeSentiment(text) {
    try {
        const model = genAI.getGenerativeModel({ model: "gemini-pro" });

        const prompt = `Analyze the sentiment of the following text and respond ONLY with a single number:
- Output -1 for negative sentiment
- Output 0 for neutral sentiment 
- Output 1 for positive sentiment

Text: "${text}"

Your response must be ONLY -1, 0, or 1.`;

        const result = await model.generateContent(prompt);
        const response = await result.response;
        const sentimentScore = response.text().trim();

        // Validate the response
        if (['-1', '0', '1'].includes(sentimentScore)) {
            return parseInt(sentimentScore);
        } else {
            throw new Error('Invalid sentiment score');
        }
    } catch (error) {
        console.error('Sentiment Analysis Error:', error);
        throw error;
    }
}

const app = express();
const port = process.env.PORT || 3000;
const genAI = new GoogleGenerativeAI(process.env.GOOGLE_API_KEY);
const recommendationService = new RecommendationService();

// Initialize Weaviate and load initial posts
app.listen(port, async () => {
    console.log(`Recommendation service listening at http://localhost:${port}`);
    await recommendationService.connectWeaviate();
    let clientReadiness = await recommendationService.weaviateClient.isReady();
    console.log(`Weaviate client ready: ${clientReadiness}`);
    await recommendationService.initializeWeaviateSchema();
    recommendationService.initCollections();
    await recommendationService.loadInitialPosts();
    let aa = await recommendationService.posts.query.nearVector( await recommendationService.generateEmbedding("controller"), {
        limit: 2,
    })
    console.log(JSON.stringify(aa));
});

app.post('/analyze-sentiment', async (req, res) => {
    try {
        const { commentText } = req.body;

        if (!commentText) {
            return res.status(400).json({ error: 'Comment text is required' });
        }

        const sentiment = await analyzeSentiment(commentText);

        res.json({
            commentText,
            sentimentScore: sentiment
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to analyze sentiment',
            details: error.message
        });
    }
});

export default RecommendationService;