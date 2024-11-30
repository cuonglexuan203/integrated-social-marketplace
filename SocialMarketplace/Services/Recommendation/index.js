import weaviate, { vectorizer, dataType } from 'weaviate-client';
import axios from 'axios';
import dotenv from 'dotenv';
import express from 'express';
import ollama from 'ollama';
import { GoogleGenerativeAI } from "@google/generative-ai";
dotenv.config();

class RecommendationService {
    constructor() {
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
                apiEndpoint: process.env.EMBEDDING_MODEL_URL,
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

    async generateVectorEmbedding(post) {
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
        return vector;
    }

    /**
     * Add or update post in Weaviate
     */
    async upsertPostVector(post) {
        try {
            const vector = await this.generateVectorEmbedding(post);
            if (!vector) return;

            // Upsert post vector to database
            const postVectorId = await this.posts.data.insert({
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

            return postVectorId;
        } catch (error) {
            console.error('Error upserting post vector:', error);
        }
    }

    /**
     * Delete post vector from Weaviate
     */
    async deletePostVectorByPostId(postId) {
        try {
            const result = await this.posts.data.deleteMany(
                this.posts.filter.byProperty("postId").equal(postId));
            console.log("Deletion result:");
            console.log(result)
        } catch (error) {
            console.error('Error deleting post vector:', error);
        }
    }

    /**
     * Load initial posts into Weaviate
     */
    async loadInitialPosts() {
        try {
            // if (await this.posts.length()) return;
            const response = await axios.get(`${process.env.FEED_SERVICE}/api/v1/Post/GetPosts?pageSize=1000`);
            const posts = response.data.result?.data;
            console.log(posts)
            for (const post of posts) {
                await this.upsertPostVector(post);
            }
        } catch (error) {
            console.error('Error loading initial posts:', error);
        }
    }

    async getRelevantPosts(post, limit = 10) {
        const postVector = await this.generateVectorEmbedding(post);
        if (!postVector) return [];
        const relevantPosts = await this.posts.query.nearVector(postVector, {
            limit,
        });
        return relevantPosts?.objects?.map(p => p.properties.postId) ?? [];
    }

    async searchPosts(keyword, limit = 10) {
        const result = await this.posts.query.nearVector(await this.generateEmbedding(keyword), {
            // alpha: 0.25,
            limit
        })
        return result?.objects?.map(p => p.properties.postId) ?? [];
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

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.post('/search-relavant-posts', async (req, res) => {
    try {
        const { keyword } = req.body;

        if (!keyword) {
            return res.status(400).json({ error: 'Keyword is required' });
        }
        const relavantPosts = await recommendationService.searchPosts(keyword);
        res.json({
            keyword,
            relavantPosts
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to get relavant posts',
            details: error.message
        });
    }
})

app.post('/get-relavant-posts', async (req, res) => {
    try {
        const post = req.body;

        if (!post) {
            return res.status(400).json({ error: 'Post is required' });
        }
        const relavantPosts = await recommendationService.getRelevantPosts(post);
        res.json({
            basePost: post,
            relavantPosts
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to get relavant posts',
            details: error.message
        });
    }
})

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

app.post("/upsert-post", async (req, res) => {
    try {
        const post = req.body;
        if (!post) {
            return res.status(400).json({ error: "Post is required" });
        }

        const postVectorId = await recommendationService.upsertPostVector(post);

        res.json({
            vectorId: postVectorId
        });

    }
    catch (error) {
        res.status(500).json({ error: 'Internal server error' });
    }
})

app.get("/delete-post/:postId", async (req, res) => {
    try {
        const postId = req.params.postId;

        if (!postId) {
            return res.status(400).json({ error: 'Post ID is required' });
        }

        await recommendationService.deletePostVectorByPostId(postId);

        res.json({
            message: 'Deleted post successfully',
            postId: postId
        });
    } catch (error) {
        res.status(500).json({ error: 'Internal server error' });
    }
})

// Initialize Weaviate and load initial posts
app.listen(port, async () => {
    console.log(`Recommendation service listening at http://localhost:${port}`);

    await recommendationService.connectWeaviate();
    let clientReadiness = await recommendationService.weaviateClient.isReady();
    console.log(`Weaviate client ready: ${clientReadiness}`);

    await recommendationService.initializeWeaviateSchema();
    recommendationService.initCollections();
    // await recommendationService.loadInitialPosts();
});

export default RecommendationService;