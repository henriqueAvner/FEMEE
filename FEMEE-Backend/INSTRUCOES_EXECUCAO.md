# Instruções para Execução Local - FEMEE Backend

Este documento descreve os passos necessários para rodar a API localmente após as correções aplicadas.

## Pré-requisitos

1.  **.NET 8 SDK**: O projeto utiliza o .NET 8. Certifique-se de ter o SDK instalado.
2.  **SQL Server (Docker)**: O banco de dados deve estar rodando conforme as credenciais fornecidas.

## Configurações Aplicadas

O arquivo `src/FEMEE.API/appsettings.json` foi atualizado com:
- **String de Conexão**: Apontando para `localhost,1433` com o banco `DB_FEMEE`.
- **JWT Secret Key**: Configurada com a chave secreta da Federação Mineira.

## Como Rodar

1.  Abra o terminal na pasta raiz do projeto.
2.  Navegue até o diretório da API:
    ```bash
    cd src/FEMEE.API
    ```
3.  Restaure as dependências e compile o projeto:
    ```bash
    dotnet build
    ```
4.  Execute a aplicação:
    ```bash
    dotnet run
    ```

## Acessando a API

- **Swagger (Documentação)**: [http://localhost:5205/swagger/index.html](http://localhost:5205/swagger/index.html)
- **Health Check**: [http://localhost:5205/health](http://localhost:5205/health)

> **Nota**: Se você encontrar erros de conexão com o banco de dados, verifique se o container Docker está acessível e se as credenciais no `appsettings.json` coincidem com a sua configuração local.
