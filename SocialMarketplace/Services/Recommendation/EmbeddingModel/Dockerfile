FROM ollama/ollama AS ollama-stage

# Copy the script to the docker image
COPY ./Services/Recommendation/EmbeddingModel/entrypoint.sh /entrypoint.sh

# Ensure the script is executable
RUN chmod +x /entrypoint.sh

# Expose the Ollama service port
EXPOSE 11434

# Use the combined entrypoint script
ENTRYPOINT ["/bin/sh", "/entrypoint.sh"]