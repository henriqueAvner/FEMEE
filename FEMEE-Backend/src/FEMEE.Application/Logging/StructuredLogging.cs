using Serilog.Context;

namespace FEMEE.Application.Logging
{
    /// <summary>
    /// Classe auxiliar para logging estruturado com contexto.
    /// Facilita o rastreamento de operações em toda a aplicação.
    /// </summary>
    public static class StructuredLogging
    {
        /// <summary>
        /// Inicia um escopo de logging com contexto.
        /// </summary>
        /// <param name="operationName">Nome da operação</param>
        /// <param name="userId">ID do usuário (opcional)</param>
        /// <returns>IDisposable para usar com using</returns>
        public static IDisposable BeginOperationScope(string operationName, int? userId = null)
        {
            var logContext = LogContext.PushProperty("Operation", operationName);

            if (userId.HasValue)
            {
                LogContext.PushProperty("UserId", userId.Value);
            }

            return logContext;
        }

        /// <summary>
        /// Adiciona contexto de usuário ao log.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <param name="email">Email do usuário</param>
        public static void AddUserContext(int userId, string email)
        {
            LogContext.PushProperty("UserId", userId);
            LogContext.PushProperty("UserEmail", email);
        }

        /// <summary>
        /// Adiciona contexto de entidade ao log.
        /// </summary>
        /// <param name="entityType">Tipo da entidade</param>
        /// <param name="entityId">ID da entidade</param>
        public static void AddEntityContext(string entityType, int entityId)
        {
            LogContext.PushProperty("EntityType", entityType);
            LogContext.PushProperty("EntityId", entityId);
        }

        /// <summary>
        /// Adiciona contexto de performance ao log.
        /// </summary>
        /// <param name="operationName">Nome da operação</param>
        /// <param name="durationMs">Duração em milissegundos</param>
        public static void LogPerformance(string operationName, long durationMs)
        {
            LogContext.PushProperty("Operation", operationName);
            LogContext.PushProperty("DurationMs", durationMs);
        }
    }
}
