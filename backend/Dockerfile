# Dockerfile para EduPlatform API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos de projeto
COPY ["EduPlatform.API/EduPlatform.API/EduPlatform.API.csproj", "EduPlatform.API/EduPlatform.API/"]
COPY ["EduPlatform.Application/EduPlatform.Application/EduPlatform.Application.csproj", "EduPlatform.Application/EduPlatform.Application/"]
COPY ["EduPlatform.Core/EduPlatform.Core/EduPlatform.Core.csproj", "EduPlatform.Core/EduPlatform.Core/"]
COPY ["EduPlatform.Infrastructure/EduPlatform.Infrastructure/EduPlatform.Infrastructure.csproj", "EduPlatform.Infrastructure/EduPlatform.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "EduPlatform.API/EduPlatform.API/EduPlatform.API.csproj"

# Copiar código fonte
COPY . .
WORKDIR "/src/EduPlatform.API/EduPlatform.API"

# Build da aplicação
RUN dotnet build "EduPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EduPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Criar usuário não-root para segurança
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "EduPlatform.API.dll"]
