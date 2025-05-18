# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and all projects
COPY ./MerchStore.sln .
COPY ./src ./src
COPY ./tests ./tests  

# Restore NuGet packages
RUN dotnet restore MerchStore.sln

# Build and publish the WebUI project
WORKDIR /src/src/MerchStore.WebUI
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MerchStore.WebUI.dll"]
