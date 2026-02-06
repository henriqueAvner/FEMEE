namespace FEMEE.API.Middleware
{
    /// <summary>
    /// Resposta padronizada de erro.
    /// Usada pelo middleware de tratamento de exceções.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Código HTTP da resposta (400, 401, 404, 500, etc).
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem de erro amigável ao usuário.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Detalhes técnicos do erro (apenas em desenvolvimento).
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Data e hora do erro em UTC.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ID único da requisição para rastreamento.
        /// </summary>
        public string? TraceId { get; set; }
    }
}
