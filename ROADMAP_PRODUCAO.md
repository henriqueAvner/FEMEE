# 🚀 ROADMAP TÉCNICO - FEMEE Arena Hub

> **Objetivo**: Evoluir o projeto do estado atual até uma aplicação pronta para produção
> **Data de Criação**: 05/02/2026
> **Última Atualização**: 05/02/2026
> **Estimativa Total**: 15-20 dias de desenvolvimento

---

## 📊 Visão Geral das Fases

| Fase | Descrição | Estimativa | Status |
|------|-----------|------------|--------|
| 1 | Correções Críticas | 2-3 dias | ✅ Concluída |
| 2 | Melhorias Arquiteturais | 4-5 dias | ✅ Concluída |
| 3 | Integração Frontend ↔ Backend | 5-7 dias | ✅ Concluída |
| 4 | Preparação para Produção | 3-4 dias | ✅ Concluída |

---

## FASE 1: CORREÇÕES CRÍTICAS ✅ CONCLUÍDA
> Prioridade: 🔴 ALTA | Estimativa: 2-3 dias | **Status: FINALIZADA**

### 1.1 Backend - Duplicações no Program.cs

- [x] **1.1.1 Remover registro duplicado de IUserService** ✅
  - **Problema**: `IUserService` estava registrado duas vezes no container DI
  - **Solução aplicada**: Removida a segunda ocorrência, mantido comentário explicativo
  - **Arquivo modificado**: `FEMEE-Backend/src/FEMEE.API/Program.cs`

- [x] **1.1.2 Remover chamada duplicada de AddAuthorizationPolicies** ✅
  - **Problema**: `AddAuthorizationPolicies()` era chamado duas vezes
  - **Solução aplicada**: Removida segunda chamada, mantido comentário explicativo
  - **Arquivo modificado**: `FEMEE-Backend/src/FEMEE.API/Program.cs`

---

### 1.2 Backend - Queries Ineficientes (N+1 e Full Table Scans)

- [x] **1.2.1 Otimizar AuthService.LoginAsync()** ✅
  - **Problema**: Carregava TODOS os usuários para encontrar um por email
  - **Solução aplicada**: Agora usa `GetByEmailAsync()` - query direta O(1)
  - **Arquivo modificado**: `FEMEE.Application/Services/Auth/AuthService.cs`

- [x] **1.2.2 Otimizar AuthService.RegisterAsync()** ✅ (BÔNUS)
  - **Problema**: Verificava email carregando todos usuários
  - **Solução aplicada**: Agora usa `EmailExistsAsync()` - query direta O(1)
  - **Arquivo modificado**: `FEMEE.Application/Services/Auth/AuthService.cs`

- [x] **1.2.3 Otimizar TimeService.GetTimeBySlugAsync()** ✅
  - **Problema**: Carregava TODOS os times para encontrar um por slug
  - **Solução aplicada**: Agora usa `GetBySlugAsync()` do repositório
  - **Arquivo modificado**: `FEMEE.Application/Services/TimeService.cs`

- [ ] **1.2.4 Otimizar JogoService.GetJogoBySlugAsync()**
  - **Problema**: Mesmo padrão de full table scan
  - **Ação**: Adicionar `GetBySlugAsync` no repositório de Jogos
  - **Prioridade**: 🟠 Média
  - **Impacto**: Performance em listagem de jogos

---

### 1.3 Backend - Validadores Faltando

- [x] **1.3.1 Injetar UpdateNoticiaValidator no controller** ✅
  - **Problema**: Endpoint PUT de Notícias não validava entrada
  - **Solução aplicada**: Validator já existia, agora está injetado e sendo usado
  - **Arquivo modificado**: `FEMEE.API/Controllers/NoticiasController.cs`

- [x] **1.3.2 Injetar UpdateProdutoValidator no controller** ✅
  - **Problema**: Endpoint PUT de Produtos não validava entrada
  - **Solução aplicada**: Validator já existia, agora está injetado e sendo usado
  - **Arquivo modificado**: `FEMEE.API/Controllers/ProdutosController.cs`

---

### 1.4 Frontend - Formulário Não Funcional

- [ ] **1.4.1 Corrigir RegistrationDialog para enviar dados**
  - **Problema**: Formulário de inscrição não faz chamada HTTP
  ```tsx
  // ATUAL - só mostra toast, não envia nada
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    toast({ title: "Inscrição realizada!" });
  };
  ```
  - **Ação**: Implementar chamada real à API (será completado na Fase 3)
  - **Arquivo**: `femee-arena-hub/src/components/forms/RegistrationDialog.tsx`
  - **Prioridade**: 🔴 Alta
  - **Impacto**: Funcionalidade core não funciona

---

## FASE 2: MELHORIAS ARQUITETURAIS - EM PROGRESSO 🔄
> Prioridade: 🟠 MÉDIA | Estimativa: 4-5 dias | **Status: PARCIALMENTE CONCLUÍDA**

### 2.1 Backend - Organização de Código

- [x] **2.1.1 Criar interface IAuthService** ✅
  - **Solução aplicada**: Criada interface `IAuthService` com métodos LoginAsync, RegisterAsync e ChangePasswordAsync
  - **Arquivo criado**: `FEMEE.Application/Interfaces/Services/IAuthService.cs`
  - **AuthService** agora implementa a interface

- [x] **2.1.2 Mover FinishPartidaRequest para pasta DTOs** ✅
  - **Solução aplicada**: Criado `FinishPartidaRequestDto` na pasta correta
  - **Arquivo criado**: `FEMEE.Application/DTOs/Partida/FinishPartidaRequestDto.cs`
  - **PartidasController** atualizado para usar o novo DTO

- [x] **2.1.3 Usar JwtSettings para expiração de token** ✅
  - **Solução aplicada**: AuthService agora injeta `IOptions<JwtSettings>` e usa `_jwtSettings.ExpirationMinutes`
  - **Arquivo modificado**: `FEMEE.Application/Services/Auth/AuthService.cs`

---

### 2.2 Backend - Paginação

- [x] **2.2.1 Criar classe base PagedResult<T>** ✅
  - **Solução aplicada**: Criado DTO genérico com Items, TotalCount, Page, PageSize, TotalPages, HasPreviousPage, HasNextPage
  - **Arquivo criado**: `FEMEE.Application/DTOs/Common/PagedResult.cs`

- [ ] **2.2.2 Implementar paginação em TimesController**
  - **Ação**: Adicionar query params `page` e `pageSize` no GET `/api/times`
  - **Prioridade**: 🟠 Média

- [ ] **2.2.3 Implementar paginação em JogadoresController**
  - **Prioridade**: 🟠 Média

- [ ] **2.2.4 Implementar paginação em PartidasController**
  - **Prioridade**: 🟠 Média

- [ ] **2.2.5 Implementar paginação em CampeonatosController**
  - **Prioridade**: 🟠 Média

---

### 2.3 Backend - Segurança

- [ ] **2.3.1 Implementar Rate Limiting**
  - **Problema**: Endpoints de auth vulneráveis a brute force
  - **Ação**: 
    1. Instalar `AspNetCoreRateLimit`
    2. Configurar limites para `/api/auth/login` e `/api/auth/register`
    ```csharp
    // appsettings.json
    "IpRateLimiting": {
      "EnableEndpointRateLimiting": true,
      "GeneralRules": [
        { "Endpoint": "*:/api/auth/*", "Period": "1m", "Limit": 10 }
      ]
    }
    ```
  - **Prioridade**: 🟠 Média
  - **Impacto**: Segurança contra ataques automatizados

- [ ] **2.3.2 Remover política CORS "AllowAll"**
  - **Problema**: Política permissiva definida (risco em produção)
  - **Ação**: Configurar origens específicas permitidas
  - **Arquivo**: `FEMEE.API/Program.cs`
  - **Prioridade**: 🟠 Média
  - **Impacto**: Segurança CORS

---

### 2.4 Frontend - Organização

- [x] **2.4.1 Adicionar React.StrictMode** ✅
  - **Solução aplicada**: StrictMode adicionado no main.tsx
  - **Arquivo modificado**: `femee-arena-hub/src/main.tsx`

- [x] **2.4.2 Configurar QueryClient com defaults otimizados** ✅ (BÔNUS)
  - **Solução aplicada**: QueryClient configurado com staleTime, retry e refetchOnWindowFocus
  - **Arquivo modificado**: `femee-arena-hub/src/App.tsx`

- [ ] **2.4.3 Unificar sistema de Toast**
  - **Problema**: Dois sistemas instalados (Radix + Sonner)
  - **Ação**: Escolher Sonner (mais moderno) e remover Radix Toast
  - **Prioridade**: 🟡 Baixa

- [ ] **2.4.4 Remover dependências não utilizadas**
  - **Prioridade**: 🟡 Baixa

---

## FASE 3: INTEGRAÇÃO FRONTEND ↔ BACKEND
> Prioridade: 🔴 ALTA | Estimativa: 5-7 dias

### 3.1 Infraestrutura de Comunicação

- [x] **3.1.1 Criar configuração de ambiente** ✅
  - **Solução aplicada**: Criados arquivos `.env` e `.env.example` com VITE_API_URL
  - **Arquivos criados**: 
    - `femee-arena-hub/.env`
    - `femee-arena-hub/.env.example`
  ```
  - **Arquivos criados**: 
    - `femee-arena-hub/.env`
    - `femee-arena-hub/.env.example`

- [x] **3.1.2 Criar instância Axios configurada** ✅
  - **Solução aplicada**: Axios configurado com interceptors de auth e error handling
  - **Arquivo criado**: `femee-arena-hub/src/services/api.ts`
  - **Dependência adicionada**: `axios` no package.json

- [x] **3.1.3 Criar types alinhados com DTOs do backend** ✅
  - **Solução aplicada**: Interfaces TypeScript para todos os DTOs (Auth, Time, Campeonato, Noticia, etc.)
  - **Arquivo criado**: `femee-arena-hub/src/types/api.types.ts`

---

### 3.2 Camada de Serviços

- [x] **3.2.1 Criar authService** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/auth.service.ts`

- [x] **3.2.2 Criar timesService** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/times.service.ts`

- [x] **3.2.3 Criar campeonatosService** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/campeonatos.service.ts`

- [x] **3.2.4 Criar noticiasService** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/noticias.service.ts`

- [x] **3.2.5 Criar inscricoesService** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/inscricoes.service.ts`

- [x] **3.2.6 Criar barrel export** ✅
  - **Arquivo criado**: `femee-arena-hub/src/services/index.ts`

---

### 3.3 React Query Hooks

- [x] **3.3.1 Configurar QueryClient** ✅
  - **Solução aplicada**: Configurado em App.tsx com staleTime, retry e refetchOnWindowFocus

- [x] **3.3.2 Criar useTimes hook** ✅
  - **Arquivo criado**: `femee-arena-hub/src/hooks/api/useTimes.ts`
  - **Hooks**: useTimes, useTime, useTimeBySlug, useTimesRanking, useCreateTime, useUpdateTime, useDeleteTime

- [x] **3.3.3 Criar useCampeonatos hook** ✅
  - **Arquivo criado**: `femee-arena-hub/src/hooks/api/useCampeonatos.ts`

- [x] **3.3.4 Criar useNoticias hook** ✅
  - **Arquivo criado**: `femee-arena-hub/src/hooks/api/useNoticias.ts`

- [x] **3.3.5 Criar useAuth hook com mutations** ✅
  - **Arquivo criado**: `femee-arena-hub/src/hooks/api/useAuth.ts`

- [x] **3.3.6 Criar useInscricoes mutation** ✅
  - **Arquivo criado**: `femee-arena-hub/src/hooks/api/useInscricoes.ts`

---

### 3.4 Contexto de Autenticação

- [x] **3.4.1 Criar AuthContext** ✅
  - **Arquivo criado**: `femee-arena-hub/src/contexts/AuthContext.tsx`
  - **Funcionalidades**: login, register, logout, isAuthenticated, isAdmin, isCapitao
  - **App.tsx** atualizado para incluir AuthProvider

- [ ] **3.4.2 Criar ProtectedRoute component**
  ```typescript
  // src/components/auth/ProtectedRoute.tsx
  export const ProtectedRoute = ({ children, roles }: Props) => {
    const { isAuthenticated, user } = useAuth();
    if (!isAuthenticated) return <Navigate to="/login" />;
    if (roles && !roles.includes(user.role)) return <Navigate to="/" />;
    return children;
  };
  ```
  - **Prioridade**: 🟠 Média

---

### 3.5 Componentes de UI para Estados

- [x] **3.5.1 Criar Loading components** ✅
  - **Arquivo criado**: `femee-arena-hub/src/components/ui/loading.tsx`
  - **Componentes**: LoadingSpinner, LoadingPage, LoadingCard

- [x] **3.5.2 Criar ErrorDisplay component** ✅
  - **Arquivo criado**: `femee-arena-hub/src/components/ui/error-display.tsx`
  - **Componentes**: ErrorDisplay, ErrorPage

- [x] **3.5.3 Criar EmptyState component** ✅
  - **Arquivo criado**: `femee-arena-hub/src/components/ui/empty-state.tsx`

---

### 3.6 Refatorar Páginas para Usar API

- [x] **3.6.1 Refatorar Index.tsx (Home)** ✅
  - **Solução aplicada**: Substituído mock data por `useNoticiasRecentes()`, `useCampeonatosAtivos()`
  - **Adicionados**: Loading spinners, error states, empty states
  - **Arquivo modificado**: `femee-arena-hub/src/pages/Index.tsx`

- [x] **3.6.2 Refatorar Times.tsx** ✅
  - **Solução aplicada**: Implementado `useTimes()` hook
  - **Arquivo modificado**: `femee-arena-hub/src/pages/Times.tsx`

- [x] **3.6.3 Refatorar TeamDetail.tsx** ✅
  - **Solução aplicada**: Implementado `useTimeBySlug()` com dados reais do backend
  - **Arquivo modificado**: `femee-arena-hub/src/pages/TeamDetail.tsx`

- [x] **3.6.4 Refatorar Ranking.tsx** ✅
  - **Solução aplicada**: Implementado `useTimesRanking()` hook
  - **Arquivo modificado**: `femee-arena-hub/src/pages/Ranking.tsx`

- [x] **3.6.5 Refatorar Campeonatos.tsx** ✅
  - **Solução aplicada**: Implementado `useCampeonatos()` hook
  - **Arquivo modificado**: `femee-arena-hub/src/pages/Campeonatos.tsx`

- [x] **3.6.6 Refatorar RegistrationDialog.tsx** ✅
  - **Solução aplicada**: 
    1. Implementado `useCreateInscricao()` mutation
    2. Adicionado tratamento de estados loading/error
    3. Validação para usuário autenticado
  - **Arquivo modificado**: `femee-arena-hub/src/components/forms/RegistrationDialog.tsx`

- [x] **3.6.7 Refatorar TeamRanking.tsx** ✅
  - **Solução aplicada**: Implementado `useTimesRanking()` hook na sidebar
  - **Arquivo modificado**: `femee-arena-hub/src/components/features/TeamRanking.tsx`

---

### 3.7 Navegação Mobile (Pendente)

- [ ] **3.3.1 Configurar QueryClient**
  ```typescript
  // src/lib/queryClient.ts
  export const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 5 * 60 * 1000, // 5 minutos
        retry: 1,
        refetchOnWindowFocus: false,
      },
    },
  });
  ```
  - **Prioridade**: 🔴 Alta

- [ ] **3.3.2 Criar useTimes hook**
  ```typescript
  // src/hooks/api/useTimes.ts
  export const useTimes = (params?: PaginationParams) => {
    return useQuery({
      queryKey: ['times', params],
      queryFn: () => timesService.getAll(params),
    });
  };

  export const useTimeBySlug = (slug: string) => {
    return useQuery({
      queryKey: ['time', slug],
      queryFn: () => timesService.getBySlug(slug),
      enabled: !!slug,
    });
  };
  ```
  - **Prioridade**: 🔴 Alta

- [ ] **3.3.3 Criar useCampeonatos hook**
  - **Prioridade**: 🔴 Alta

- [ ] **3.3.4 Criar useNoticias hook**
  - **Prioridade**: 🔴 Alta

- [ ] **3.3.5 Criar useAuth hook com mutations**
  ```typescript
  export const useLogin = () => {
    return useMutation({
      mutationFn: authService.login,
      onSuccess: (data) => {
        localStorage.setItem('token', data.token);
        queryClient.invalidateQueries({ queryKey: ['user'] });
      },
    });
  };
  ```
  - **Prioridade**: 🔴 Alta

- [ ] **3.3.6 Criar useInscricao mutation**
  - **Prioridade**: 🔴 Alta

---

### 3.4 Contexto de Autenticação

- [ ] **3.4.1 Criar AuthContext**
  ```typescript
  // src/contexts/AuthContext.tsx
  interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (data: LoginRequest) => Promise<void>;
    logout: () => void;
  }
  ```
  - **Arquivo**: `femee-arena-hub/src/contexts/AuthContext.tsx`
  - **Prioridade**: 🔴 Alta
  - **Impacto**: Gerenciamento de sessão do usuário

- [ ] **3.4.2 Criar ProtectedRoute component**
  ```typescript
  // src/components/auth/ProtectedRoute.tsx
  export const ProtectedRoute = ({ children, roles }: Props) => {
    const { isAuthenticated, user } = useAuth();
    if (!isAuthenticated) return <Navigate to="/login" />;
    if (roles && !roles.includes(user.role)) return <Navigate to="/" />;
    return children;
  };
  ```
  - **Prioridade**: 🟠 Média

---

### 3.5 Refatorar Páginas para Usar API

- [ ] **3.5.1 Refatorar Index.tsx (Home)**
  - **Ação**: Substituir mock data por `useNoticias()`, `useTimes()`, `useCampeonatos()`
  - **Adicionar**: Loading skeletons, error states
  - **Prioridade**: 🔴 Alta

- [ ] **3.5.2 Refatorar Times.tsx**
  - **Ação**: Usar `useTimes()` hook
  - **Prioridade**: 🔴 Alta

- [ ] **3.5.3 Refatorar TeamDetail.tsx**
  - **Ação**: Usar `useTimeBySlug()` com dados reais do backend
  - **Prioridade**: 🔴 Alta

- [ ] **3.5.4 Refatorar Ranking.tsx**
  - **Ação**: Usar `useTimesRanking()` hook
  - **Prioridade**: 🔴 Alta

- [ ] **3.5.5 Refatorar Campeonatos.tsx**
  - **Ação**: Usar `useCampeonatos()` hook
  - **Prioridade**: 🔴 Alta

- [ ] **3.5.6 Refatorar RegistrationDialog.tsx**
  - **Ação**: 
    1. Migrar para React Hook Form + Zod
    2. Usar `useInscricao()` mutation
    3. Tratar estados de loading/error
  - **Prioridade**: 🔴 Alta

---

### 3.6 Componentes de UI para Estados

- [ ] **3.6.1 Criar Skeleton components**
  ```typescript
  // src/components/ui/skeletons/
  - TeamCardSkeleton.tsx
  - NewsCardSkeleton.tsx
  - ChampionshipCardSkeleton.tsx
  - RankingRowSkeleton.tsx
  ```
  - **Prioridade**: 🟠 Média
  - **Impacto**: UX durante carregamento

- [ ] **3.6.2 Criar ErrorBoundary global**
  ```typescript
  // src/components/error/ErrorBoundary.tsx
  // src/components/error/ErrorFallback.tsx
  ```
  - **Prioridade**: 🟠 Média
  - **Impacto**: Tratamento gracioso de erros

- [ ] **3.6.3 Criar EmptyState component**
  - **Ação**: Componente para listas vazias
  - **Prioridade**: 🟡 Baixa

---

### 3.7 Navegação Mobile

- [ ] **3.7.1 Implementar menu hamburger no Header**
  - **Problema**: Nav hidden em mobile (`hidden md:flex`)
  - **Ação**: 
    1. Usar hook `useMobile` existente
    2. Adicionar Sheet/Drawer para menu mobile
    3. Implementar toggle button
  - **Arquivo**: `femee-arena-hub/src/components/layout/Header.tsx`
  - **Prioridade**: 🟠 Média
  - **Impacto**: Acessibilidade mobile

---

## FASE 4: PREPARAÇÃO PARA PRODUÇÃO
> Prioridade: 🟠 MÉDIA | Estimativa: 3-4 dias

### 4.1 Backend - Configurações de Produção

- [ ] **4.1.1 Configurar CORS para produção**
  ```csharp
  // Program.cs
  builder.Services.AddCors(options => {
    options.AddPolicy("Production", policy => {
      policy.WithOrigins("https://femee.com.br", "https://www.femee.com.br")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
  });
  ```
  - **Prioridade**: 🔴 Alta
  - **Impacto**: Segurança

- [ ] **4.1.2 Configurar User Secrets / Azure Key Vault**
  - **Ação**: Garantir que secrets não estão no código
  - Verificar: JWT SecretKey, Connection Strings
  - **Prioridade**: 🔴 Alta

- [ ] **4.1.3 Configurar Health Checks detalhados**
  ```csharp
  builder.Services.AddHealthChecks()
    .AddDbContextCheck<FemeeDbContext>()
    .AddCheck("memory", () => /* memory check */)
    .AddCheck("disk", () => /* disk check */);
  ```
  - **Prioridade**: 🟠 Média

- [ ] **4.1.4 Habilitar Swagger apenas em Development**
  ```csharp
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }
  ```
  - **Prioridade**: 🟠 Média

- [ ] **4.1.5 Configurar logging para produção**
  - **Ação**: Configurar Serilog para enviar logs para serviço externo (Application Insights, Seq, etc.)
  - **Prioridade**: 🟠 Média

---

### 4.2 Frontend - Build e Otimizações

- [ ] **4.2.1 Configurar variáveis de ambiente de produção**
  ```env
  # .env.production
  VITE_API_URL=https://api.femee.com.br
  VITE_APP_NAME=FEMEE Arena Hub
  ```
  - **Prioridade**: 🔴 Alta

- [ ] **4.2.2 Configurar PWA (opcional)**
  - **Ação**: Adicionar `vite-plugin-pwa` para funcionamento offline
  - **Prioridade**: 🟡 Baixa

- [ ] **4.2.3 Otimizar bundle splitting**
  ```typescript
  // vite.config.ts
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          vendor: ['react', 'react-dom', 'react-router-dom'],
          ui: ['@radix-ui/react-dialog', '@radix-ui/react-dropdown-menu'],
        }
      }
    }
  }
  ```
  - **Prioridade**: 🟠 Média

- [ ] **4.2.4 Adicionar lazy loading nas rotas**
  ```typescript
  const Times = lazy(() => import('./pages/Times'));
  const Campeonatos = lazy(() => import('./pages/Campeonatos'));
  ```
  - **Prioridade**: 🟠 Média

- [ ] **4.2.5 Configurar compressão de assets**
  - **Ação**: Adicionar `vite-plugin-compression` para gzip/brotli
  - **Prioridade**: 🟡 Baixa

---

### 4.3 Testes

- [ ] **4.3.1 Aumentar cobertura de testes unitários no backend**
  - **Meta**: 80% de cobertura em Services
  - **Prioridade**: 🟠 Média

- [ ] **4.3.2 Adicionar testes de integração na API**
  - **Ação**: Usar WebApplicationFactory para testes E2E dos endpoints
  - **Arquivo**: `FEMEE-Backend/tests/FEMEE.IntegrationTests/`
  - **Prioridade**: 🟠 Média

- [ ] **4.3.3 Configurar testes no frontend**
  - **Ação**: 
    1. Instalar Vitest + Testing Library
    2. Criar testes para hooks e componentes críticos
  - **Prioridade**: 🟠 Média

- [ ] **4.3.4 Configurar testes E2E com Playwright**
  - **Ação**: Fluxos críticos: Login, Inscrição, Navegação
  - **Prioridade**: 🟡 Baixa

---

### 4.4 CI/CD

- [ ] **4.4.1 Criar pipeline de CI (GitHub Actions)**
  ```yaml
  # .github/workflows/ci.yml
  - Build backend
  - Run backend tests
  - Build frontend
  - Run frontend tests
  - Lint checks
  ```
  - **Prioridade**: 🟠 Média

- [ ] **4.4.2 Criar pipeline de CD**
  - **Ação**: Deploy automático para staging/production
  - **Prioridade**: 🟠 Média

- [ ] **4.4.3 Configurar Docker para produção**
  - **Ação**: Otimizar Dockerfile existente
  - Multi-stage build para menor imagem
  - **Arquivo**: `FEMEE-Backend/Dockerfile`
  - **Prioridade**: 🟠 Média

---

### 4.5 Documentação

- [ ] **4.5.1 Documentar endpoints da API**
  - **Ação**: Adicionar XML comments para Swagger
  - **Prioridade**: 🟡 Baixa

- [ ] **4.5.2 Criar README.md atualizado**
  - **Ação**: Instruções de setup, arquitetura, deploy
  - **Prioridade**: 🟡 Baixa

- [ ] **4.5.3 Documentar variáveis de ambiente**
  - **Ação**: Criar `.env.example` com todas as variáveis necessárias
  - **Prioridade**: 🟡 Baixa

---

### 4.6 Monitoramento

- [ ] **4.6.1 Configurar Application Insights / Sentry**
  - **Ação**: Tracking de erros em produção
  - **Prioridade**: 🟠 Média

- [ ] **4.6.2 Configurar métricas de performance**
  - **Ação**: Tempo de resposta, throughput, error rate
  - **Prioridade**: 🟡 Baixa

---

## 📋 RESUMO DE TAREFAS POR PRIORIDADE

### 🔴 ALTA (Bloqueadores)
| # | Tarefa | Fase |
|---|--------|------|
| 1 | Remover registros duplicados Program.cs | 1 |
| 2 | Otimizar AuthService.LoginAsync | 1 |
| 3 | Otimizar TimeService.GetBySlug | 1 |
| 4 | Adicionar validadores faltando | 1 |
| 5 | Criar camada de serviços frontend | 3 |
| 6 | Criar types alinhados com DTOs | 3 |
| 7 | Implementar React Query hooks | 3 |
| 8 | Refatorar páginas para usar API | 3 |
| 9 | Criar AuthContext | 3 |
| 10 | Configurar CORS produção | 4 |
| 11 | Configurar env de produção | 4 |

### 🟠 MÉDIA (Importantes)
| # | Tarefa | Fase |
|---|--------|------|
| 1 | Criar IAuthService interface | 2 |
| 2 | Implementar paginação | 2 |
| 3 | Implementar rate limiting | 2 |
| 4 | Criar ProtectedRoute | 3 |
| 5 | Criar Skeleton components | 3 |
| 6 | Criar ErrorBoundary | 3 |
| 7 | Implementar menu mobile | 3 |
| 8 | Configurar health checks | 4 |
| 9 | Testes unitários/integração | 4 |
| 10 | CI/CD pipelines | 4 |

### 🟡 BAIXA (Nice to have)
| # | Tarefa | Fase |
|---|--------|------|
| 1 | Mover FinishPartidaRequest | 2 |
| 2 | Adicionar StrictMode | 2 |
| 3 | Unificar sistema toast | 2 |
| 4 | Remover deps não usadas | 2 |
| 5 | Configurar PWA | 4 |
| 6 | Documentação | 4 |

---

## ⏱️ CRONOGRAMA SUGERIDO

```
Semana 1: Fase 1 (Correções Críticas) + Início Fase 2
├── Dias 1-2: Items 1.1, 1.2, 1.3
├── Dias 3-4: Item 1.4 + Items 2.1, 2.2
└── Dia 5: Items 2.3, 2.4

Semana 2: Fase 3 (Integração)
├── Dias 1-2: Items 3.1, 3.2 (Infraestrutura + Services)
├── Dias 3-4: Items 3.3, 3.4 (React Query + Auth)
└── Dia 5: Items 3.5 (Refatorar páginas - parte 1)

Semana 3: Fase 3 (continuação) + Fase 4
├── Dias 1-2: Items 3.5, 3.6, 3.7 (Finalizar integração)
├── Dias 3-4: Items 4.1, 4.2 (Configs produção)
└── Dia 5: Items 4.3, 4.4 (Testes + CI/CD)

Semana 4: Buffer + Deploy
├── Dias 1-2: Ajustes finais e bug fixes
├── Dia 3: Deploy staging
├── Dia 4: Testes em staging
└── Dia 5: Deploy produção
```

---

## ✅ CHECKLIST PRÉ-DEPLOY

- [ ] Todas as correções críticas aplicadas
- [ ] Integração frontend ↔ backend funcionando
- [ ] Variáveis de ambiente configuradas
- [ ] CORS configurado para domínio de produção
- [ ] Secrets removidos do código
- [ ] Health check respondendo
- [ ] Testes passando
- [ ] Build de produção funcionando
- [ ] Monitoramento configurado
- [ ] Backup de banco configurado
- [ ] SSL/HTTPS configurado
- [ ] DNS configurado
