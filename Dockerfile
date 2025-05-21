# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY ./MerchStore.sln .
COPY ./src ./src
COPY ./tests ./tests  

RUN dotnet restore MerchStore.sln

# Build and publish the application
WORKDIR /src/src/MerchStore.WebUI
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app from build stage
COPY --from=build /app/publish .

# Configure environment variables and port
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "MerchStore.WebUI.dll"]
