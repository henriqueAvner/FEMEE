# 🎮 FEMEE API - Backend para Plataforma de eSports

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Tests](https://img.shields.io/badge/Tests-331%20Passed-4CAF50?style=for-the-badge&logo=checkmarx&logoColor=white)

**API RESTful completa para gerenciamento de campeonatos de eSports, times, jogadores e muito mais.**

[Início Rápido](#-início-rápido) •
[Documentação](#-documentação-da-api) •
[Arquitetura](#-arquitetura) •
[Endpoints](#-endpoints) •
[Testes](#-testes)

</div>

---

## 📋 Índice

- [Sobre o Projeto](#-sobre-o-projeto)
- [Tecnologias](#-tecnologias)
- [Início Rápido](#-início-rápido)
- [Configuração](#-configuração)
- [Arquitetura](#-arquitetura)
- [Endpoints](#-endpoints)
- [Autenticação](#-autenticação)
- [Testes](#-testes)
- [Docker](#-docker)
- [Contribuição](#-contribuição)

---

## 🎯 Sobre o Projeto

A **FEMEE API** é o backend para uma plataforma completa de eSports que permite:

- 🏆 **Gerenciamento de Campeonatos** - Criar, gerenciar e acompanhar torneios
- 👥 **Gestão de Times** - Cadastro de equipes com capitães e jogadores
- 🎮 **Jogos Suportados** - League of Legends, Counter-Strike 2, EA FC
- 📰 **Notícias** - Sistema de publicação de conteúdo
- 🛒 **Loja** - Gerenciamento de produtos
- 🔐 **Autenticação JWT** - Sistema seguro com políticas de acesso

---

## 🛠 Tecnologias

| Categoria | Tecnologia |
|-----------|------------|
| **Framework** | .NET 10.0 |
| **ORM** | Entity Framework Core 10 |
| **Banco de Dados** | SQL Server 2022 |
| **Autenticação** | JWT Bearer Tokens |
| **Validação** | FluentValidation 12.x |
| **Mapeamento** | AutoMapper 12.x |
| **Logs** | Serilog |
| **Documentação** | Swagger / OpenAPI |
| **Testes** | xUnit + Moq |
| **Container** | Docker |

---

## 🚀 Início Rápido

### Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2022](https://www.microsoft.com/sql-server) ou [Docker](https://www.docker.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/FEMEE-Backend.git
cd FEMEE-Backend
```

### 2. Inicie o SQL Server (Docker)

```bash
docker-compose up -d
```

### 3. Configure os segredos

```bash
cd src/FEMEE.API

# Configurar connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=FemeeDb;User Id=sa;Password=F_CLAN@GG123!;TrustServerCertificate=True"

# Configurar chave JWT (mínimo 32 caracteres)
dotnet user-secrets set "JwtSettings:SecretKey" "SuaChaveSecretaSuperSeguraComMaisDe32Caracteres!"
```

### 4. Execute as migrações

```bash
cd src/FEMEE.API
dotnet ef database update --project ../FEMEE.Infrastructure
```

### 5. Execute a aplicação

```bash
dotnet run
```

A API estará disponível em:
- **HTTP:** http://localhost:5299
- **HTTPS:** https://localhost:7299
- **Swagger:** http://localhost:5299/swagger

---

## ⚙️ Configuração

### Variáveis de Ambiente

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `ConnectionStrings__DefaultConnection` | String de conexão SQL Server | `Server=...;Database=FemeeDb;...` |
| `JwtSettings__SecretKey` | Chave secreta para JWT (min. 32 chars) | `MinhaChaveSecreta...` |
| `JwtSettings__Issuer` | Emissor do token | `https://femee-api.com` |
| `JwtSettings__Audience` | Audiência do token | `femee-frontend` |
| `JwtSettings__ExpirationMinutes` | Tempo de expiração | `60` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente de execução | `Development` / `Production` |

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "JwtSettings": {
    "SecretKey": "",
    "Issuer": "https://femee-api.com",
    "Audience": "femee-frontend",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "AllowedOrigins": [
    "https://femee-arena-hub.com",
    "http://localhost:3000",
    "http://localhost:5173"
  ]
}
```

> ⚠️ **IMPORTANTE:** Nunca commite segredos no repositório. Use User Secrets em desenvolvimento e variáveis de ambiente em produção.

---

## 🏗 Arquitetura

O projeto segue os princípios da **Clean Architecture**:

```
FEMEE-Backend/
├── src/
│   ├── FEMEE.API/              # Camada de Apresentação
│   │   ├── Controllers/        # Controllers REST
│   │   ├── Middleware/         # Exception Handling, Request Logging
│   │   └── Configuration/      # Swagger, Logging configs
│   │
│   ├── FEMEE.Application/      # Camada de Aplicação
│   │   ├── DTOs/               # Data Transfer Objects
│   │   ├── Interfaces/         # Contratos de serviços e repositórios
│   │   ├── Services/           # Implementação de regras de negócio
│   │   ├── Validators/         # Validações com FluentValidation
│   │   └── Mappings/           # Configuração AutoMapper
│   │
│   ├── FEMEE.Domain/           # Camada de Domínio
│   │   ├── Entities/           # Entidades do domínio
│   │   ├── Enums/              # Enumerações
│   │   └── Interfaces/         # Contratos do domínio
│   │
│   └── FEMEE.Infrastructure/   # Camada de Infraestrutura
│       ├── Data/               # DbContext, Repositories
│       ├── Migrations/         # Migrações EF Core
│       ├── Security/           # Password Hasher (BCrypt)
│       └── Extensions/         # Authorization Policies
│
└── tests/
    ├── FEMEE.UnitTests/        # Testes unitários (331 testes)
    └── FEMEE.IntegrationTests/ # Testes de integração
```

### Entidades Principais

| Entidade | Descrição |
|----------|-----------|
| `User` | Usuários do sistema (Admin, Capitão, Jogador) |
| `Time` | Equipes/times de eSports |
| `Jogador` | Jogadores vinculados a times |
| `Campeonato` | Torneios e competições |
| `Partida` | Jogos entre times em campeonatos |
| `Jogo` | Títulos de jogos (CS2, LoL, EA FC) |
| `InscricaoCampeonato` | Inscrições de times em campeonatos |
| `Conquista` | Premiações e conquistas de times |
| `Noticia` | Publicações e notícias |
| `Produto` | Itens da loja |

---

## 📡 Endpoints

### Health Check

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/health` | Status básico da API |
| `GET` | `/health/detailed` | Status detalhado com verificação de banco |
| `GET` | `/health/live` | Liveness probe (Kubernetes) |
| `GET` | `/health/ready` | Readiness probe (Kubernetes) |

### Autenticação

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `POST` | `/api/auth/login` | Login de usuário | ❌ |
| `POST` | `/api/auth/register` | Registro de novo usuário | ❌ |

### Usuários

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/users` | Listar usuários | 🔐 Admin |
| `GET` | `/api/users/{id}` | Buscar usuário por ID | 🔐 Admin |
| `PUT` | `/api/users/{id}` | Atualizar usuário | 🔐 Admin |
| `DELETE` | `/api/users/{id}` | Deletar usuário | 🔐 Admin |

### Times

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/times` | Listar todos os times | ❌ |
| `GET` | `/api/times/{id}` | Buscar time por ID | ❌ |
| `GET` | `/api/times/slug/{slug}` | Buscar time por slug | ❌ |
| `GET` | `/api/times/ranking` | Ranking de times | ❌ |
| `POST` | `/api/times` | Criar time | 🔐 Admin/Capitão |
| `PUT` | `/api/times/{id}` | Atualizar time | 🔐 Admin/Capitão |
| `DELETE` | `/api/times/{id}` | Deletar time | 🔐 Admin |

### Campeonatos

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/campeonatos` | Listar campeonatos | ❌ |
| `GET` | `/api/campeonatos/{id}` | Buscar por ID | ❌ |
| `GET` | `/api/campeonatos/status/{status}` | Filtrar por status | ❌ |
| `POST` | `/api/campeonatos` | Criar campeonato | 🔐 Admin |
| `PUT` | `/api/campeonatos/{id}` | Atualizar campeonato | 🔐 Admin |
| `DELETE` | `/api/campeonatos/{id}` | Deletar campeonato | 🔐 Admin |

### Jogadores

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/jogadores` | Listar jogadores | ❌ |
| `GET` | `/api/jogadores/{id}` | Buscar por ID | ❌ |
| `GET` | `/api/jogadores/time/{timeId}` | Jogadores de um time | ❌ |
| `POST` | `/api/jogadores` | Criar jogador | 🔐 Admin/Capitão |
| `PUT` | `/api/jogadores/{id}` | Atualizar jogador | 🔐 Admin/Capitão |
| `DELETE` | `/api/jogadores/{id}` | Deletar jogador | 🔐 Admin |

### Partidas

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/partidas` | Listar partidas | ❌ |
| `GET` | `/api/partidas/{id}` | Buscar por ID | ❌ |
| `GET` | `/api/partidas/campeonato/{id}` | Partidas de um campeonato | ❌ |
| `POST` | `/api/partidas` | Criar partida | 🔐 Admin |
| `PUT` | `/api/partidas/{id}` | Atualizar partida | 🔐 Admin |
| `PUT` | `/api/partidas/{id}/resultado` | Registrar resultado | 🔐 Admin |
| `DELETE` | `/api/partidas/{id}` | Deletar partida | 🔐 Admin |

### Jogos

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/jogos` | Listar jogos | ❌ |
| `GET` | `/api/jogos/{id}` | Buscar por ID | ❌ |
| `GET` | `/api/jogos/ativos` | Jogos ativos | ❌ |
| `POST` | `/api/jogos` | Criar jogo | 🔐 Admin |
| `PUT` | `/api/jogos/{id}` | Atualizar jogo | 🔐 Admin |
| `DELETE` | `/api/jogos/{id}` | Deletar jogo | 🔐 Admin |

### Inscrições em Campeonatos

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/inscricoescampeonato` | Listar inscrições | 🔐 Admin |
| `GET` | `/api/inscricoescampeonato/campeonato/{id}` | Por campeonato | 🔐 Admin |
| `POST` | `/api/inscricoescampeonato` | Criar inscrição | 🔐 Capitão |
| `PUT` | `/api/inscricoescampeonato/{id}/aprovar` | Aprovar inscrição | 🔐 Admin |
| `PUT` | `/api/inscricoescampeonato/{id}/rejeitar` | Rejeitar inscrição | 🔐 Admin |

### Conquistas

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/conquistas` | Listar conquistas | ❌ |
| `GET` | `/api/conquistas/time/{timeId}` | Por time | ❌ |
| `POST` | `/api/conquistas` | Criar conquista | 🔐 Admin |
| `DELETE` | `/api/conquistas/{id}` | Deletar conquista | 🔐 Admin |

### Notícias

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/noticias` | Listar notícias | ❌ |
| `GET` | `/api/noticias/publicadas` | Notícias publicadas | ❌ |
| `GET` | `/api/noticias/{id}` | Buscar por ID | ❌ |
| `POST` | `/api/noticias` | Criar notícia | 🔐 Admin |
| `PUT` | `/api/noticias/{id}` | Atualizar notícia | 🔐 Admin |
| `DELETE` | `/api/noticias/{id}` | Deletar notícia | 🔐 Admin |

### Produtos

| Método | Endpoint | Descrição | Auth |
|--------|----------|-----------|------|
| `GET` | `/api/produtos` | Listar produtos | ❌ |
| `GET` | `/api/produtos/ativos` | Produtos ativos | ❌ |
| `GET` | `/api/produtos/{id}` | Buscar por ID | ❌ |
| `POST` | `/api/produtos` | Criar produto | 🔐 Admin |
| `PUT` | `/api/produtos/{id}` | Atualizar produto | 🔐 Admin |
| `DELETE` | `/api/produtos/{id}` | Deletar produto | 🔐 Admin |

---

## 🔐 Autenticação

A API utiliza **JWT (JSON Web Tokens)** para autenticação.

### Tipos de Usuário

| Tipo | Valor | Permissões |
|------|-------|------------|
| `Administrador` | 1 | Acesso total a todos os recursos |
| `Capitao` | 2 | Gerenciamento do próprio time |
| `Jogador` | 3 | Acesso limitado |

### Políticas de Autorização

| Política | Descrição |
|----------|-----------|
| `AdminOnly` | Apenas administradores |
| `AdminOrCapitao` | Administradores ou capitães |
| `UsuarioAutenticado` | Qualquer usuário logado |
| `CapitaoOnly` | Apenas capitães |
| `JogadorOnly` | Apenas jogadores |

### Exemplo de Requisição Autenticada

```bash
# 1. Fazer login
curl -X POST http://localhost:5299/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@femee.com", "senha": "Admin@123"}'

# Resposta: {"token": "eyJhbGciOiJIUzI1NiIs...", "userId": 1, ...}

# 2. Usar o token nas próximas requisições
curl -X GET http://localhost:5299/api/users \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."
```

---

## 🧪 Testes

O projeto conta com **331 testes unitários** cobrindo:

- ✅ Controllers
- ✅ Services
- ✅ Validators
- ✅ Mappings
- ✅ Middleware
- ✅ Entities

### Executar Testes

```bash
# Todos os testes
dotnet test

# Com cobertura detalhada
dotnet test --verbosity normal

# Apenas testes unitários
dotnet test tests/FEMEE.UnitTests

# Apenas testes de integração
dotnet test tests/FEMEE.IntegrationTests
```

### Estrutura de Testes

```
tests/
├── FEMEE.UnitTests/
│   ├── Controllers/      # Testes de controllers
│   ├── Services/         # Testes de serviços
│   ├── Validators/       # Testes de validação
│   ├── Mappings/         # Testes de mapeamento
│   ├── Middleware/       # Testes de middleware
│   ├── Domain/           # Testes de entidades
│   └── Infrastructure/   # Testes de repositórios
│
└── FEMEE.IntegrationTests/
```

---

## 🐳 Docker

### Build da Imagem

```bash
docker build -t femee-api:latest .
```

### Executar com Docker Compose

```bash
# Subir todos os serviços (API + SQL Server)
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar serviços
docker-compose down
```

### Docker Compose Completo

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: femee-sqlserver
    environment:
      SA_PASSWORD: "F_CLAN@GG123!"
      ACCEPT_EULA: "Y"
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql

  api:
    build: .
    container_name: femee-api
    depends_on:
      - sqlserver
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=FemeeDb;User Id=sa;Password=F_CLAN@GG123!;TrustServerCertificate=True
      - JwtSettings__SecretKey=SuaChaveSecretaSuperSeguraComMaisDe32Caracteres!
    ports:
      - "8080:8080"

volumes:
  sqlserver_data:
```

### Health Checks

A API expõe endpoints de health check para orquestradores:

```bash
# Verificar se a API está saudável
curl http://localhost:8080/health

# Verificar status detalhado (inclui banco de dados)
curl http://localhost:8080/health/detailed
```

---

## 📊 Padrões de Resposta

### Sucesso

```json
{
  "id": 1,
  "nome": "Time Alpha",
  "slug": "time-alpha",
  "descricao": "...",
  "createdAt": "2026-02-04T10:00:00Z"
}
```

### Erro de Validação (400)

```json
{
  "statusCode": 400,
  "message": "Erro de validação",
  "details": "O campo 'Nome' é obrigatório; O campo 'Email' deve ser um email válido",
  "timestamp": "2026-02-04T10:00:00Z",
  "traceId": "0HN5ABCD..."
}
```

### Não Autorizado (401)

```json
{
  "statusCode": 401,
  "message": "Não autorizado",
  "details": "Token inválido ou expirado",
  "timestamp": "2026-02-04T10:00:00Z",
  "traceId": "0HN5ABCD..."
}
```

### Não Encontrado (404)

```json
{
  "statusCode": 404,
  "message": "Recurso não encontrado",
  "details": "Time com ID 999 não foi encontrado",
  "timestamp": "2026-02-04T10:00:00Z",
  "traceId": "0HN5ABCD..."
}
```

---

## 📁 Arquivos de Teste HTTP

O arquivo `femee-api.http` contém exemplos de requisições para testar a API:

```bash
# Abrir no VS Code com a extensão REST Client
code femee-api.http
```

---

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/nova-feature`)
3. Commit suas mudanças (`git commit -m 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 📞 Suporte

- 📧 Email: suporte@femee.com
- 💬 Discord: [FEMEE Community](https://discord.gg/femee)
- 🐛 Issues: [GitHub Issues](https://github.com/seu-usuario/FEMEE-Backend/issues)

---

<div align="center">

**Feito com ❤️ pela equipe FEMEE**

</div>
