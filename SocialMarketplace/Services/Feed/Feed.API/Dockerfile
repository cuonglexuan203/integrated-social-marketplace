#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Feed/Feed.API/Feed.API.csproj", "Services/Feed/Feed.API/"]
COPY ["Services/Feed/Feed.Infrastructure/Feed.Infrastructure.csproj", "Services/Feed/Feed.Infrastructure/"]
COPY ["Services/Feed/Feed.Application/Feed.Application.csproj", "Services/Feed/Feed.Application/"]
COPY ["Services/Feed/Feed.Core/Feed.Core.csproj", "Services/Feed/Feed.Core/"]
RUN dotnet restore "./Services/Feed/Feed.API/Feed.API.csproj"
COPY . .
WORKDIR "/src/Services/Feed/Feed.API"
RUN dotnet build "./Feed.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Feed.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feed.API.dll"]