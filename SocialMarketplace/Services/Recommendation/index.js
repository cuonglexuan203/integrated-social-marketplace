import weaviate, { vectorizer, dataType } from 'weaviate-client';
import axios from 'axios';
import dotenv from 'dotenv';
import express from 'express';
import { Ollama } from 'ollama';
import { GoogleGenerativeAI } from "@google/generative-ai";
import cors from "cors";
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
    async initWeaviateSchema() {

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

    initEmbeddingModel() {
        this.ollama = new Ollama({
            host: process.env.EMBEDDING_MODEL_URL,
        })
    }

    /**
     * Generate embedding using nomic-embed-text
     */
    async generateEmbedding(text) {
        try {
            const response = await this.ollama.embeddings({
                model: 'nomic-embed-text',
                prompt: text,
            });
            return response.embedding;
        } catch (error) {
            console.error('Error generating embedding:', error.message);
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
            if (!vector) {
                throw new Error(`No vector generated for post ID: ${post.id}`);
            }

            // Upsert post vector to database
            const postVectorId = await this.posts.data.insert({
                metadata: post.id,
                properties: {
                    contentText: post.contentText,
                    tags: post.tags,
                    category: post.category,
                    postId: post.id
                },
                // id: post.id,
                vectors: vector,

            });

            return postVectorId;
        } catch (error) {
            console.error('Error upserting post vector:', error.message);
            throw error;
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
            if (await this.posts.length()) return;
            const response = await axios.get(`${process.env.FEED_SERVICE}/api/v1/Post/GetPosts?pageSize=1000`);
            const posts = response.data.result?.data;
            console.log(`Fetched posts count: ${(posts.length)}`);

            for (const post of posts) {
                let retryCount = 0;
                const MAX_RETRIES = 6;

                while (retryCount < MAX_RETRIES) {
                    try {
                        await this.upsertPostVector(post);
                        break; // Successfully upserted, exit the retry loop
                    } catch (upsertError) {
                        retryCount++;

                        if (retryCount >= MAX_RETRIES) {
                            console.error(`Failed to upsert post ${post.id} after ${MAX_RETRIES} attempts.`, upsertError);
                            // Optionally log to an error tracking system or handle critical failure
                            break;
                        }

                        console.error(`Attempt ${retryCount}: Failed to upsert post ${post.id}. Retrying in 30 seconds...`, upsertError);

                        // Wait for 30 seconds before retrying
                        await new Promise(resolve => setTimeout(resolve, 30000));
                    }
                }
                
                if (retryCount >= MAX_RETRIES){
                    console.error("🔴Loading posts terminated");
                    break;
                }
            }
            console.info("✅ Loading initial posts successfully!")
        } catch (error) {
            console.error('Error loading initial posts:', error);
        }
    }

    async getRelevantPosts(postId, limit = 10, offset = 0) {
        const existingPosts = await this.posts.query.fetchObjects({
            filters: this.posts.filter.byProperty("postId").equal(postId),
            includeVector: true,
            limit: 1,
        });

        if (existingPosts?.objects?.length === 0) {
            return null;
        }

        const postVector = existingPosts.objects.at(0).vectors.default;
        const relevantPosts = await this.posts.query.nearVector(postVector, {
            limit,
            offset
        });

        return relevantPosts?.objects?.map(p => p.properties.postId) ?? [];

        // return relevantPosts?.objects ?? [];
    }

    async searchPosts(keyword, limit = 10, offset = 0) {
        const result = await this.posts.query.bm25(keyword, {
            limit,
            offset,
        })
        return result?.objects?.map(p => p.properties.postId)  ?? [];

        // return result?.objects ?? [];
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

app.use(cors({
    origin: '*', // Allows any origin
    methods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS'], // Allowed HTTP methods
    allowedHeaders: ['Content-Type', 'Authorization'] // Allowed headers
}));

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.use((req, res, next) => {
    const logMessage = `📥 Incoming request: 🛑 Method - ${req.method} 🛤️ Path - ${req.path}`;
    console.log(logMessage);
    next();
});

app.post('/search-relevant-posts', async (req, res) => {
    try {
        const { keyword, limit, offset } = req.body;

        if (!keyword) {
            return res.status(400).json({ error: 'Keyword is required' });
        }
        const relevantPostIds = await recommendationService.searchPosts(keyword, limit, offset);
        res.json({
            keyword,
            relevantPostIds
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to get relevant posts',
            details: error.message
        });
    }
})

app.get('/get-relevant-posts', async (req, res, next) => {
    try {
        const { postId, limit, offset } = req.query;

        if (!postId) {
            return res.status(400).json({ error: 'Post ID is required' });
        }

        const relevantPostIds = await recommendationService.getRelevantPosts(postId, limit, offset);

        if (!relevantPostIds) {
            const error = new Error(`Post not found: post id ${postId}`);
            error.status = 404;
            return next(error);
        }

        res.json({
            basePostId: postId,
            relevantPostIds
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to get relevant posts',
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

app.delete("/delete-post/:postId", async (req, res) => {
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

app.use((req, res, next) => {
    const error = new Error("Not found");
    error.status = 404;
    next(error);
})

app.use((err, req, res, next) => {
    res.status(err.status || 500).json({
        error: {
            message: err.message,
        },
    });
})

// Initialize Weaviate and load initial posts
app.listen(port, async () => {
    console.log(`🚀 Recommendation service is running on port ${port} 🌐`);

    await recommendationService.connectWeaviate();
    let clientReadiness = await recommendationService.weaviateClient.isReady();
    console.log(`Weaviate client ready: ${clientReadiness}`);

    await recommendationService.initWeaviateSchema();
    recommendationService.initCollections();
    recommendationService.initEmbeddingModel();
    await recommendationService.loadInitialPosts();
});

export default RecommendationService;