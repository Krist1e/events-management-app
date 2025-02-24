﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventsManagementApp.Api/EventsManagementApp.Api.csproj", "EventsManagementApp.Api/"]
COPY ["EventsManagementApp.Infrastructure/EventsManagementApp.Infrastructure.csproj", "EventsManagementApp.Infrastructure/"]
COPY ["EventManagementApp.Domain/EventManagementApp.Domain.csproj", "EventManagementApp.Domain/"]
COPY ["EventsManagementApp.Application/EventsManagementApp.Application.csproj", "EventsManagementApp.Application/"]
RUN dotnet restore "EventsManagementApp.Api/EventsManagementApp.Api.csproj"
COPY . .
WORKDIR "/src/EventsManagementApp.Api"
RUN dotnet build "EventsManagementApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EventsManagementApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsManagementApp.Api.dll"]
