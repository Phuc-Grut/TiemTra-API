# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files
COPY ["src/TiemTra_Api/TiemTra_Api.csproj", "TiemTra_Api/"]
COPY ["src/Application/Application.csproj", "Application/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["src/Shared/Shared.csproj", "Shared/"]

# Restore
WORKDIR /src/TiemTra_Api
RUN dotnet restore "TiemTra_Api.csproj"

# Copy toàn bộ src (sau restore)
WORKDIR /src
COPY src/ ./

# Build
WORKDIR /src/TiemTra_Api
RUN dotnet build "TiemTra_Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TiemTra_Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TiemTra_Api.dll"]
