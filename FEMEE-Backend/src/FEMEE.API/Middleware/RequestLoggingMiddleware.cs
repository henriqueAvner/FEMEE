using System.Diagnostics;

namespace FEMEE.API.Middleware
{
    /// <summary>
    /// Middleware que registra todas as requisições HTTP e respostas.
    /// Inclui informações de tempo de execução e status code.
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Construtor do middleware.
        /// </summary>
        /// <param name="next">Próximo middleware na pipeline</param>
        /// <param name="logger">Logger para registrar requisições</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invoca o middleware.
        /// Registra informações da requisição e resposta.
        /// </summary>
        /// <param name="context">Contexto HTTP</param>
        public async Task InvokeAsync(HttpContext context)
        {
            // ===== REGISTRAR INFORMAÇÕES DA REQUISIÇÃO =====
            var request = context.Request;
            var stopwatch = Stopwatch.StartNew();

            // Ler o corpo da requisição se for POST/PUT
            string requestBody = "";
            if (request.ContentLength > 0 && IsRequestBodyLoggable(request.ContentType ?? string.Empty))
            {
                request.EnableBuffering();
                using (var reader = new StreamReader(request.Body, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            _logger.LogInformation(
                "Requisição iniciada: {Method} {Path} | " +
                "IP: {RemoteIP} | " +
                "TraceId: {TraceId}",
                request.Method,
                request.Path,
                context.Connection.RemoteIpAddress,
                context.TraceIdentifier);

            if (!string.IsNullOrEmpty(requestBody) && requestBody.Length < 1000)
            {
                _logger.LogDebug("Corpo da requisição: {RequestBody}", requestBody);
            }

            // ===== CAPTURAR RESPOSTA =====
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                finally
                {
                    stopwatch.Stop();

                    // ===== REGISTRAR INFORMAÇÕES DA RESPOSTA =====
                    _logger.LogInformation(
                        "Requisição concluída: {Method} {Path} | " +
                        "Status: {StatusCode} | " +
                        "Tempo: {ElapsedMilliseconds}ms | " +
                        "TraceId: {TraceId}",
                        request.Method,
                        request.Path,
                        context.Response.StatusCode,
                        stopwatch.ElapsedMilliseconds,
                        context.TraceIdentifier);

                    // ===== ALERTAR SE REQUISIÇÃO DEMOROU MUITO =====
                    if (stopwatch.ElapsedMilliseconds > 5000)
                    {
                        _logger.LogWarning(
                            "Requisição lenta detectada: {Method} {Path} | " +
                            "Tempo: {ElapsedMilliseconds}ms",
                            request.Method,
                            request.Path,
                            stopwatch.ElapsedMilliseconds);
                    }

                    // ===== ALERTAR SE STATUS CODE FOR ERRO =====
                    if (context.Response.StatusCode >= 400)
                    {
                        _logger.LogWarning(
                            "Resposta com erro: {Method} {Path} | " +
                            "Status: {StatusCode}",
                            request.Method,
                            request.Path,
                            context.Response.StatusCode);
                    }

                    // ===== COPIAR RESPOSTA PARA STREAM ORIGINAL =====
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                    context.Response.Body = originalBodyStream;
                }
            }
        }

        /// <summary>
        /// Verifica se o tipo de conteúdo deve ter seu corpo registrado.
        /// </summary>
        /// <param name="contentType">Tipo de conteúdo da requisição</param>
        /// <returns>True se deve registrar, false caso contrário</returns>
        private static bool IsRequestBodyLoggable(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;

            // Registrar apenas JSON e form data
            return contentType.Contains("application/json") ||
                   contentType.Contains("application/x-www-form-urlencoded");
        }
    }
}
