FROM node:18-alpine

WORKDIR /app

# Copy all files from the specific service directory to the working directory
COPY ./Services/Recommendation/ ./

# Install dependencies
RUN npm install

# Expose the application port
EXPOSE 3000

# Run the application
CMD ["npm", "start"]