using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.Auth
{
    /// <summary>
    /// DTO para requisição de registro de novo usuário.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Email do usuário.
        /// Deve ser único no sistema.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Senha em texto plano.
        /// Será hasheada no servidor.
        /// </summary>
        public string? Senha { get; set; }

        /// <summary>
        /// Confirmação de senha.
        /// Deve ser igual a Senha.
        /// </summary>
        public string? ConfirmacaoSenha { get; set; }

        /// <summary>
        /// Telefone do usuário (opcional).
        /// </summary>
        public string? Telefone { get; set; }

        /// <summary>
        /// Tipo de usuário ao registrar.
        /// Padrão: Jogador
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; } = TipoUsuario.Jogador;
    }
}
