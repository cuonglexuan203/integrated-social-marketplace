# The Integrated Social Marketplace (Freedom Market)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Frontend: Angular](https://img.shields.io/badge/Frontend-Angular%2018-red.svg?logo=angular)](https://angular.io/)
[![Backend: .NET](https://img.shields.io/badge/Backend-.NET%208-purple.svg?logo=.net)](https://dotnet.microsoft.com/)
[![AI: Google Gemini](https://img.shields.io/badge/AI-Google%20Gemini-blue.svg?logo=google)](https://ai.google.dev/gemini-api)
[![Vector DB: Weaviate](https://img.shields.io/badge/Vector%20DB-Weaviate-green.svg?logo=weaviate)](https://weaviate.io/)
[![API Gateway: Ocelot](https://img.shields.io/badge/API%20Gateway-Ocelot-orange.svg)](https://github.com/ThreeMammals/Ocelot)
[![Database: SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server)
[![Database: MongoDB](https://img.shields.io/badge/Database-MongoDB-47A248?logo=mongodb)](https://www.mongodb.com/)
[![Database: PostgreSQL](https://img.shields.io/badge/Database-PostgreSQL-4169E1?logo=postgresql)](https://www.postgresql.org/)
[![Containerization: Docker](https://img.shields.io/badge/Containerization-Docker-blue.svg?logo=docker)](https://www.docker.com/)
[![Cloud Storage: Cloudinary](https://img.shields.io/badge/Cloud%20Storage-Cloudinary-blue.svg?logo=cloudinary)](https://cloudinary.com/)

<!-- <div align="center">
  <img src="Docs/Assets/Images/social_marketplace_banner.png" alt="Social Marketplace Banner" width="700">
</div> -->

## 🚀 Overview
The Integrated Social Marketplace (also known as Freedom Market) is a platform designed to merge the dynamic interactions of a social network with the transactional capabilities of an online marketplace. It empowers users to freely post products or services, browse personalized feeds, connect directly with sellers via integrated chat, and engage with content through familiar social features.

The platform leverages AI and a modern microservice architecture to deliver an efficient, engaging, and trustworthy experience.

**Key Innovations**:
- **AI-Driven Personalization**: Personalized news feeds and product recommendations powered by AI and user behavior analysis.
- **Integrated Social Commerce**: Seamlessly combines shopping with social interactions like comments, shares, and direct messaging.
- **Advanced Scoring System**: Implements a sophisticated scoring mechanism for posts (based on interactions, sentiment, freshness) and user credibility.
- **LLM-Powered Features**: Utilizes Google Gemini for sentiment analysis of comments (contributing to post quality scores) and an interactive AI Chatbot.
- **Scalable Microservice Architecture**: Built for flexibility, scalability, and independent service deployment.

## ✨ Features
| Module | Capabilities |
|--------|--------------|
| **User Management** | Registration, Login, Profile Management (details, interests, etc.) |
| **Post Management** | Create, Update, Delete posts (products/services) with descriptions, media (images/videos), and tags. |
| **Marketplace & Feed** | Personalized content feed, Browse and search for posts/products (keywords, tags). |
| **Social Engagement** | Like, Comment, Share, and Save posts. Follow/Unfollow other users. |
| **Real-Time Chat** | Direct one-on-one messaging between users, attach posts to messages. |
| **AI Recommendation** | AI-powered suggestions for posts and products based on user preferences and interactions. Semantic search capabilities. |
| **AI Chatbot** | Interact with a Google Gemini-powered chatbot for inquiries (e.g., trending items, platform help). |
| **Trust & Safety** | User and post reporting system, Admin moderation of content, posts, users, and reports. User credibility scoring. |
| **Admin Dashboard** | Interface for administrators to manage the platform, review reports, and monitor activity. |

## 🧠 AI & Recommendation Architecture
The platform deeply integrates AI to enhance user experience:
- **Recommendation Engine**: Utilizes Weaviate as a vector database for semantic search and to store embeddings of posts. A multi-step process retrieves top interacted posts, finds semantically similar posts via Weaviate, and then ranks them using the comprehensive post scoring system.
- **Scoring System**:
    - **Post Score**: Calculated based on interaction counts (reactions, comments, shares with weights), a decay factor for freshness, author-user relationship boosts, comment sentiment (via Gemini), and penalties from reports.
    - **User Credibility**: Dynamically updated based on positive/negative interactions, reporting accuracy, and engagement quality.
- **LLM Integration (Google Gemini)**:
    - **Sentiment Analysis**: Analyzes comment sentiment (positive, neutral, negative) to contribute to the post's quality score.
    - **AI Chatbot**: Powers a conversational AI assistant for users.
## 🛠️ Tech Stack

### System Architecture
<div align="center">
  <img src="Assets/Images/system-architecture.png" alt="System Architecture">
</div>

### Component Breakdown
| Component | Technologies | Key Features |
|-----------|--------------|-------------------------|
| **Client Application** | Angular 18, TypeScript | SPA, User Interface, User Profile, Feed, Chat, Notifications (planned).|
| **API Gateway** | Ocelot | Request routing.|
| **Identity Microservice** | ASP.NET Core 8, SQL Server | Manages user authentication (JWT), authorization, registration, and profiles.|
| **Feed Microservice** | ASP.NET Core 8, MongoDB | Manages posts (creation, updates, deletion), comments, reactions, tags, and personalized user feeds. Integrates with Recommendation microservice. Media links via Cloudinary.|
| **Chat Microservice** | ASP.NET Core 8, MongoDB | Handles real-time one-on-one messaging between users, chat history.|
| **Recommendation Microservice** | Express.js 4.21.1, Google Gemini SDK, Weaviate| Handles AI-driven features: personalized recommendations (semantic search with Weaviate), LLM integration (Google Gemini) for sentiment analysis and AI chatbot functionality.|
| **Notification Microservice** | ASP.NET Core 8, PostgreSQL | (Planned) Manages and delivers real-time user notifications (e.g., new messages, post interactions).|
| **Shared Infrastructure** | .NET 8 Libraries, RabbitMQ | Contains shared backend libraries and services.|
| **Databases** | SQL Server, MongoDB, Weaviate, PostgreSQL | Relational data (Identity, Notification), Document data (Feed, Chat), Vector data for AI (Recommendation).|
| **Cloud Services** | Cloudinary | Media (images, videos) storage and CDN.|

## ⚙️ Installation

### Prerequisites
*   Docker & Docker Compose
*   .NET 8 SDK
*   Node.js (latest LTS, for Angular 18) and npm/yarn
*   Access to Google Gemini API (API Key)
*   Cloudinary account (API Key, Secret)

### Running with Docker
```bash
# Clone repository
git clone https://github.com/cuonglexuan203/integrated-social-marketplace.git
cd integrated-social-marketplace

# Configure environment variables
# Create/update .env files within each service directory or use a docker-compose.override.yml for:
# - Database connection strings (SQL Server, MongoDB, PostgreSQL, Weaviate)
# - API Keys (Google Gemini, Cloudinary)
# - JWT Secret Key (for Identity Microservice)

# Start services using Docker Compose
cd SocialMarketplace
docker-compose up -d
```


## 📱 User Flows
1.  **Selling & Discovery**:
    *   Seller: Posts a new item (e.g., "Handmade Ceramic Mug") with photos, description, and price.
    *   System: AI suggests relevant tags. The post is added to the marketplace.
    *   Buyer: Browses their personalized feed or searches for "ceramic mug" and discovers the item.
2.  **Social Interaction & Transaction**:
    *   Buyer: Comments on the mug post: "What are the dimensions?"
    *   Seller: Receives a notification and replies via integrated chat, potentially sharing more details or images.
    *   Another User: Likes the post or shares it with a friend interested in ceramics.
3.  **AI-Powered Assistance** (planned):
    *   User: Opens the AI Chatbot and asks, "Show me popular gift ideas under $50."
    *   AI Chatbot (Gemini): Responds with a curated list of trending items on the platform that match the criteria.

## 📂 Project Structure
```
integrated-social-marketplace/
├── client/
├── SocialMarketplace/
│   ├── ApiGateways/
│   │   └── Ocelot.ApiGateway/
│   ├── Services/
│   │   ├── Identity/            # Manages user authentication, registration, and profiles.
│   │   ├── Feed/                # Manages posts, comments, reactions, and personalized feeds.
│   │   ├── Chat/                # Handles real-time messaging between users.
│   │   ├── Recommendation/      # Handles AI-driven features (recommendations, semantic search, chatbot).
│   │   └── Notification/        # (Planned) Manages user notifications and alerts.
│   ├── Infrastructure/
│   ├── SocialMarketplace.sln
│   └── docker-compose.yml
├── Docs/
└── README.md
```

## 🧑‍💻 Contributors
-   [Le Xuan Cuong](https://github.com/cuonglexuan203) (Backend Implementation, Architecture)
-   [Do Quang Trieu](https://github.com) (Frontend Implementation)

## 📜 License
This project is licensed under the MIT License

---
> **Ho Chi Minh City University of Technology and Education**  
> Faculty of International Education - Project on Software Engineering  
> Supervisor: Mr. Nguyen Duc Khoan  
> 2024