FROM debian:12
WORKDIR /app

# Define the build path and binary name arguments
ARG BUILD_PATH=./Builds/Release/DedicatedServer/Linux_x86_64/
ARG BINARY_NAME=DedicatedServer

# Install necessary libraries for Unity
RUN apt-get update && apt-get install -y \
    ca-certificates \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/* \
    && useradd -ms /bin/bash rivet

# Copy the precompiled Unity server files using the BUILD_PATH argument
COPY ${BUILD_PATH} /app
RUN ls /app && chmod +x /app/${BINARY_NAME}

USER rivet
ENTRYPOINT ["/app/${BINARY_NAME}", "-server", "-batchmode", "-nographics"]
