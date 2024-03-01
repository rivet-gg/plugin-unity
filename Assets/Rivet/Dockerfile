# MARK: Runner
FROM ubuntu:22.04
WORKDIR /app

# Install necessary libraries for Unity
RUN apt-get update && apt-get install -y \
    ca-certificates \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/* \
    && useradd -ms /bin/bash rivet

# Copy the precompiled Unity server files
COPY ./Builds/Game /app/Game
# COPY ./Builds/Game_BurstDebugInformation_DoNotShip /app/Game_BurstDebugInformation_DoNotShip
# COPY ./Builds/Game_Data /app/Game_Data
# COPY ./Builds/UnityPlayer.so /app/UnityPlayer.so

# Change to user rivet
USER rivet

ENV UNITY_SERVER=1

# Run the server
ENTRYPOINT ["./Game", "-batchmode", "-nographics"]
