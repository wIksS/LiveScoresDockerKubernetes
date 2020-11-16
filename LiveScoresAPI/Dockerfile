#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LiveScoresAPI/LiveScoresAPI.csproj", "LiveScoresAPI/"]
COPY ["LiveScores/LiveScores.csproj", "LiveScores/"]
RUN dotnet restore "LiveScoresAPI/LiveScoresAPI.csproj"
COPY . .
WORKDIR "/src/LiveScoresAPI"
RUN dotnet build "LiveScoresAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "LiveScoresAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LiveScoresAPI.dll"]