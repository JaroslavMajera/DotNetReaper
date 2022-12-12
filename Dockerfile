#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY ["DotNetReaper.csproj", "."]
RUN dotnet restore "./DotNetReaper.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DotNetReaper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetReaper.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN set -ex; \
    apt update && apt install -y chromium procps;

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DotNetReaper.dll"]