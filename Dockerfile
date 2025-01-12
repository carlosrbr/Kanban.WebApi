FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . .

RUN dotnet restore ./BACK/backend.sln

RUN dotnet build ./BACK/backend.sln -c release -o /app/build

FROM build AS publish
RUN dotnet publish ./BACK/backend.sln -c release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kanban.WebApi.dll"]
