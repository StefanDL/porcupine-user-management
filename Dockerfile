FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM node:20-bookworm AS ui
WORKDIR /src
COPY ["PorcupineUserManagement/ClientApp",""]
WORKDIR "/src/ClientApp"
RUN npm install  # Ensure npm packages are installed
RUN npm run build


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PorcupineUserManagement/PorcupineUserManagement.csproj", "PorcupineUserManagement/"]
RUN dotnet restore "PorcupineUserManagement/PorcupineUserManagement.csproj"
COPY . .
COPY --from=ui /src/ClientApp /src/PorcupineUserManagement/ClientApp
WORKDIR "/src/PorcupineUserManagement"
RUN dotnet build "./PorcupineUserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PorcupineUserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PorcupineUserManagement.dll"]
