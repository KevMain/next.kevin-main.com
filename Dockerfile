# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY KevinMain.API/KevinMain.API.csproj KevinMain.API/
RUN dotnet restore "KevinMain.API/KevinMain.API.csproj"

# Copy everything else and build
COPY KevinMain.API/ KevinMain.API/
WORKDIR /src/KevinMain.API
RUN dotnet build "KevinMain.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "KevinMain.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser
USER appuser

COPY --from=publish /app/publish .

# Expose port 8080 (Azure Container Apps default)
EXPOSE 8080

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "KevinMain.API.dll"]
