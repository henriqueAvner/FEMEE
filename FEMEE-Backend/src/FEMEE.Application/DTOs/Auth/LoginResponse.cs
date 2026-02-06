// FEMEE.Application/DTOs/Auth/LoginResponse.cs

using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para resposta de login.
    /// Retorna o token e informações do usuário.
    /// </summary>
    public class LoginResponse
    {
        /// <summary>
        /// Token JWT para autenticação.
        /// </summary>
        public string? Token { get; set; }

        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Nome { get; set; }

        /// <summary>
        /// Tipo de usuário (Admin, Capitao, Jogador).
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }

        /// <summary>
        /// Timestamp de expiração do token.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
