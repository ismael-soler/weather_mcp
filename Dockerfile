FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the project file first and restore dependencies.
# This is a Docker optimization. If only code files change later,
# this layer can be reused from cache, speeding up builds.
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application's code.
COPY . ./

# Publish the application
RUN dotnet publish -c Release -o /app/publish

# --- 

# Use the smaller .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

# Copy the published otuput from the build stage
COPY --from=build /app/publish .

# Set an initial comand that keeps the container alive
# Explanation: this is a Linux command that essentially does nothing, forever. 
ENTRYPOINT ["tail", "-f", "/dev/null"]