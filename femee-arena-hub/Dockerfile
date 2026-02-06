# ========================================
# STAGE 1: BUILD
# ========================================
FROM node:20-alpine AS build

WORKDIR /app

# Copiar arquivos de dependências primeiro (melhor cache)
COPY package*.json ./

# Instalar dependências
RUN npm ci --only=production=false

# Copiar código-fonte
COPY . .

# Build para produção
ARG VITE_API_URL=https://api.femee.com.br/api
ENV VITE_API_URL=$VITE_API_URL

RUN npm run build

# ========================================
# STAGE 2: RUNTIME (Nginx)
# ========================================
FROM nginx:alpine AS runtime

# Copiar configuração customizada do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copiar arquivos do build
COPY --from=build /app/dist /usr/share/nginx/html

# Expor porta
EXPOSE 80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost/ || exit 1

# Comando de inicialização
CMD ["nginx", "-g", "daemon off;"]
