# Use the .NET 9 SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Use the .NET 9 runtime for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy built app
COPY --from=build /out .

# Expose port (update based on your app)
EXPOSE 5000

# Start the application
CMD ["dotnet", "TechAssignment.dll"]
