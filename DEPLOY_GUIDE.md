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
- [ ] GitHub Secrets e Variables configurados (para CI/CD)

---

## 10. Configuração do GitHub Actions (CI/CD)

Para que o pipeline de CI/CD funcione corretamente, você precisa configurar as seguintes secrets e variables no repositório GitHub.

### 10.1 Acessar Configurações

1. Vá para o repositório no GitHub
2. Clique em **Settings** (Configurações)
3. No menu lateral, vá para **Secrets and variables** → **Actions**

### 10.2 Secrets (Obrigatórios para CI/CD completo)

| Secret | Descrição | Exemplo |
|--------|-----------|---------|
| `CODECOV_TOKEN` | Token do Codecov para upload de cobertura de testes | Obtido em [codecov.io](https://codecov.io) |

**Como criar:**
1. Na aba **Secrets**, clique em **New repository secret**
2. Nome: `CODECOV_TOKEN`
3. Value: Cole o token obtido do Codecov

### 10.3 Variables (Configurações de Build)

| Variable | Descrição | Valor Sugerido |
|----------|-----------|----------------|
| `VITE_API_URL` | URL da API para build de produção | `https://api.femee.com.br/api` |

**Como criar:**
1. Na aba **Variables**, clique em **New repository variable**
2. Nome: `VITE_API_URL`
3. Value: URL da sua API em produção

### 10.4 Secrets para Deploy (Opcional - se usar CD completo)

Se você adicionar deploy automático ao pipeline, precisará de secrets adicionais dependendo do provedor:

**Azure:**
| Secret | Descrição |
|--------|-----------|
| `AZURE_CREDENTIALS` | Service Principal JSON |
| `AZURE_SUBSCRIPTION_ID` | ID da assinatura Azure |

**AWS:**
| Secret | Descrição |
|--------|-----------|
| `AWS_ACCESS_KEY_ID` | Access Key ID |
| `AWS_SECRET_ACCESS_KEY` | Secret Access Key |

**Docker Hub:**
| Secret | Descrição |
|--------|-----------|
| `DOCKERHUB_USERNAME` | Usuário do Docker Hub |
| `DOCKERHUB_TOKEN` | Token de acesso |

### 10.5 Verificar Pipeline

Após configurar, faça um push para a branch `main` ou `develop` e verifique:
1. Vá para a aba **Actions** no repositório
2. Verifique se o workflow "CI/CD Pipeline" está executando
3. Clique no workflow para ver os logs de cada job
