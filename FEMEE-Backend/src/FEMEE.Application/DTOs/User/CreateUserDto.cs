using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.User
{
    /// <summary>
    /// DTO para criar um novo usuário.
    /// Contém apenas os dados necessários para criação.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Email do usuário (deve ser único).
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Senha em texto plano (será hasheada no servidor).
        /// </summary>
        public string Senha { get; set; } = null!;

        /// <summary>
        /// Telefone do usuário.
        /// </summary>
        public string Telefone { get; set; } = null!;

        /// <summary>
        /// Tipo de usuário (Admin, Capitao, Jogador).
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }
    }
}
