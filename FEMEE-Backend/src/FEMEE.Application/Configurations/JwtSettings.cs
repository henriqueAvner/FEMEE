using System;

namespace FEMEE.Application.Configurations
{
    public class JwtSettings
    {
        /// <summary>
        /// Chave secreta usada para assinar o token.
        /// IMPORTANTE: Deve ter pelo menos 32 caracteres para segurança.
        /// Nunca deve ser hardcoded - usar variáveis de ambiente.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Issuer (emissor) do token.
        /// Identifica quem emitiu o token.
        /// Exemplo: "https://femee-api.com"
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Audience (audiência ) do token.
        /// Identifica para quem o token foi emitido.
        /// Exemplo: "femee-frontend"
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração do token em minutos.
        /// Exemplo: 60 minutos
        /// </summary>
        public int ExpirationMinutes { get; set; }

        /// <summary>
        /// Tempo de expiração do refresh token em dias.
        /// Exemplo: 7 dias
        /// </summary>
        public int RefreshTokenExpirationDays { get; set; }
    }
}
