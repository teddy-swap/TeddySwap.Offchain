# Use the official Microsoft .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore any dependencies (via NuGet)
COPY ["/src/TeddySwap.Sync/TeddySwap.Sync.csproj", "./"]
RUN dotnet restore "TeddySwap.Sync.csproj"

# Copy the project files and build the application
COPY . .
WORKDIR "/app/src/TeddySwap.Sync"
RUN dotnet build "TeddySwap.Sync.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "TeddySwap.Sync.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeddySwap.Sync.dll"]
