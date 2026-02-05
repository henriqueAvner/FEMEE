# 🚀 FEMEE Arena Hub - Guia de Deploy para Produção

## Pré-requisitos

- Docker e Docker Compose instalados
- Acesso ao repositório Git
- Configuração de variáveis de ambiente

---

## 1. Configuração de Variáveis de Ambiente

### 1.1 Criar arquivo `.env` na raiz do projeto

```bash
cp .env.example .env
```

### 1.2 Editar `.env` com valores reais

```env
# Database
DB_CONNECTION_STRING=Server=db;Database=DB_FEMEE;User Id=sa;Password=SUA_SENHA_FORTE;TrustServerCertificate=True;
DB_SA_PASSWORD=SUA_SENHA_FORTE_AQUI

# JWT (mínimo 32 caracteres)
JWT_SECRET_KEY=sua-chave-jwt-super-segura-com-mais-de-32-caracteres

# Frontend
VITE_API_URL=https://api.femee.com.br/api
```

**⚠️ IMPORTANTE:** Use senhas fortes (mínimo 12 caracteres, letras maiúsculas, minúsculas, números e símbolos).

---

## 2. Deploy com Docker Compose

### 2.1 Build e iniciar containers

```bash
# Build das imagens
docker-compose build

# Iniciar em modo detached
docker-compose up -d
```

### 2.2 Verificar status dos containers

```bash
docker-compose ps
docker-compose logs -f api
```

### 2.3 Executar migrations do banco de dados

```bash
# Acessar o container da API
docker exec -it femee-api /bin/bash

# Ou executar migrations via dotnet CLI (localmente)
cd FEMEE-Backend
dotnet ef database update --project src/FEMEE.Infrastructure --startup-project src/FEMEE.API
```

---

## 3. Verificação de Saúde

### 3.1 Health Check da API

```bash
curl http://localhost:5000/health
```

### 3.2 Testar endpoints

```bash
# Login de teste
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@femee.com.br","senha":"senha123"}'

# Campeonatos ativos
curl http://localhost:5000/api/campeonatos/ativos
```

---

## 4. URLs de Acesso

| Serviço | URL Local | Porta |
|---------|-----------|-------|
| Frontend | http://localhost:8080 | 8080 |
| API | http://localhost:5000 | 5000 |
| Swagger | http://localhost:5000/swagger | 5000 |
| SQL Server | localhost:1434 | 1434 |

---

## 5. Deploy em Cloud (Exemplo Azure)

### 5.1 Azure Container Instances

```bash
# Login no Azure
az login

# Criar resource group
az group create --name femee-rg --location brazilsouth

# Deploy do container
az container create \
  --resource-group femee-rg \
  --name femee-api \
  --image femee-api:latest \
  --dns-name-label femee-api \
  --ports 8080 \
  --environment-variables \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__DefaultConnection="$DB_CONNECTION_STRING" \
    JwtSettings__SecretKey="$JWT_SECRET_KEY"
```

### 5.2 Azure App Service (Alternativa)

```bash
# Deploy via CLI
az webapp create --resource-group femee-rg --plan femee-plan --name femee-api --deployment-container-image-name femee-api:latest
```

---

## 6. Monitoramento

### 6.1 Logs em tempo real

```bash
docker-compose logs -f --tail=100
```

### 6.2 Métricas de containers

```bash
docker stats
```

---

## 7. Backup do Banco de Dados

```bash
# Backup
docker exec femee-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "$DB_SA_PASSWORD" \
  -Q "BACKUP DATABASE [DB_FEMEE] TO DISK = N'/var/opt/mssql/backup/DB_FEMEE.bak'"

# Copiar backup para host
docker cp femee-db:/var/opt/mssql/backup/DB_FEMEE.bak ./backup/
```

---

## 8. Troubleshooting

### Erro: Container não inicia

```bash
docker-compose logs api
docker-compose logs db
```

### Erro: Conexão com banco de dados

Verificar se o SQL Server está healthy:
```bash
docker-compose ps db
docker exec femee-db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$DB_SA_PASSWORD" -Q "SELECT 1"
```

### Erro: JWT inválido

Verificar se a chave JWT tem pelo menos 32 caracteres:
```bash
echo -n "$JWT_SECRET_KEY" | wc -c
```

---

## 9. Atualização da Aplicação

```bash
# Pull das últimas alterações
git pull origin main

# Rebuild e restart
docker-compose down
docker-compose build --no-cache
docker-compose up -d

# Aplicar migrations (se houver)
dotnet ef database update --project src/FEMEE.Infrastructure --startup-project src/FEMEE.API
```

---

## Checklist Pré-Deploy

- [ ] Arquivo `.env` configurado com senhas fortes
- [ ] CORS configurado para domínios de produção
- [ ] SSL/TLS configurado (HTTPS)
- [ ] Backup do banco de dados existente
- [ ] Health checks funcionando
- [ ] Logs configurados para persistência
- [ ] Monitoramento ativo
