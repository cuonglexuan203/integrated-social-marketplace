import express from 'express';
import { GoogleGenerativeAI } from "@google/generative-ai";
import dotenv from 'dotenv';

// Load environment variables
dotenv.config();

const app = express();
const port = process.env.PORT || 3000;

// Middleware to parse JSON bodies
app.use(express.json());

// Initialize Google Gemini
const genAI = new GoogleGenerativeAI(process.env.GOOGLE_API_KEY);

// Sentiment analysis function
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

// Sentiment Analysis Endpoint
app.post('/analyze-sentiment', async (req, res) => {
    try {
        const { contentText } = req.body;

        if (!contentText) {
            return res.status(400).json({ error: 'Comment text is required' });
        }

        const sentiment = await analyzeSentiment(contentText);

        res.json({
            contentText,
            sentimentScore: sentiment
        });
    } catch (error) {
        res.status(500).json({
            error: 'Failed to analyze sentiment',
            details: error.message
        });
    }
});

// Start the server
app.listen(port, () => {
    console.log(`Sentiment Analysis Server running on port ${port}`);
});