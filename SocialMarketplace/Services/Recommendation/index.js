import express from 'express';

const app = express();
const port = process.env.PORT || 3000;
const recommendationService = new RecommendationService();

// Initialize Weaviate and load initial posts
app.listen(port, async () => {
    console.log(`Recommendation service listening at http://localhost:${port}`);
    await recommendationService.initializeWeaviateSchema();
    await recommendationService.loadInitialPosts();
});

// Get recommendations endpoint
app.get('/recommendations/:userId', async (req, res) => {
    try {
        const recommendations = await recommendationService.getRelevantPosts(
            req.params.userId,
            parseInt(req.query.limit) || 20
        );
        res.json(recommendations);
    } catch (error) {
        console.error('Error getting recommendations:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// Update post endpoint
app.put('/posts/:postId', async (req, res) => {
    try {
        await recommendationService.upsertPostVector(req.body);
        res.json({ success: true });
    } catch (error) {
        console.error('Error updating post:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});

// Delete post endpoint
app.delete('/posts/:postId', async (req, res) => {
    try {
        await recommendationService.deletePostVector(req.params.postId);
        res.json({ success: true });
    } catch (error) {
        console.error('Error deleting post:', error);
        res.status(500).json({ error: 'Internal server error' });
    }
});