# ========================================
# STAGE 1: BUILD
# ========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copiar arquivos de projeto
COPY ["femee-api.sln", "./"]
COPY ["src/FEMEE.API/FEMEE.API.csproj", "src/FEMEE.API/"]
COPY ["src/FEMEE.Application/FEMEE.Application.csproj", "src/FEMEE.Application/"]
COPY ["src/FEMEE.Domain/FEMEE.Domain.csproj", "src/FEMEE.Domain/"]
COPY ["src/FEMEE.Infrastructure/FEMEE.Infrastructure.csproj", "src/FEMEE.Infrastructure/"]
COPY ["tests/FEMEE.UnitTests/FEMEE.UnitTests.csproj", "tests/FEMEE.UnitTests/"]
COPY ["tests/FEMEE.IntegrationTests/FEMEE.IntegrationTests.csproj", "tests/FEMEE.IntegrationTests/"]

# Restaurar dependências
RUN dotnet restore "femee-api.sln"

# Copiar código-fonte
COPY . .

# Build da aplicação
RUN dotnet build "femee-api.sln" -c Release --no-restore

# Publicar a aplicação
RUN dotnet publish "src/FEMEE.API/FEMEE.API.csproj" -c Release -o /app/publish --no-build

# ========================================
# STAGE 2: RUNTIME
# ========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Criar usuário não-root por segurança
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Copiar arquivos publicados do stage anterior
COPY --from=build /app/publish .

# Expor porta padrão
EXPOSE 8080
EXPOSE 8443

# Variáveis de ambiente padrão
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Comando de inicialização
ENTRYPOINT ["dotnet", "FEMEE.API.dll"]
