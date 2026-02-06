using System.Text.Json;
using FluentValidation;

namespace FEMEE.API.Middleware
{
    /// <summary>
    /// Middleware que captura exceções não tratadas e retorna respostas de erro padronizadas.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Construtor do middleware.
        /// </summary>
        /// <param name="next">Próximo middleware na pipeline</param>
        /// <param name="logger">Logger para registrar exceções</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invoca o middleware.
        /// Captura exceções e as trata de forma centralizada.
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção não tratada ocorreu");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Trata a exceção e escreve a resposta no contexto HTTP.
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        /// <param name="exception">Exceção capturada</param>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier
            };

            // Tratar diferentes tipos de exceção
            switch (exception)
            {
                // ===== VALIDAÇÃO =====
                case ValidationException validationEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Erro de validação";
                    response.Details = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage));
                    break;

                // ===== NÃO AUTORIZADO =====
                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.StatusCode = 401;
                    response.Message = "Não autorizado";
                    response.Details = exception.Message;
                    break;

                // ===== NÃO ENCONTRADO =====
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.StatusCode = 404;
                    response.Message = "Recurso não encontrado";
                    response.Details = exception.Message;
                    break;

                // ===== OPERAÇÃO INVÁLIDA =====
                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Operação inválida";
                    response.Details = exception.Message;
                    break;

                // ===== ARGUMENTO INVÁLIDO =====
                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.StatusCode = 400;
                    response.Message = "Argumento inválido";
                    response.Details = exception.Message;
                    break;

                // ===== ERRO GENÉRICO =====
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.StatusCode = 500;
                    response.Message = "Erro interno do servidor";
                    response.Details = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.";
                    break;
            }

            // Serializar resposta para JSON
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(json);
        }
    }
}
