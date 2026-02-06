using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.User
{
    /// <summary>
    /// DTO para atualizar um usuário existente.
    /// Contém apenas os dados que podem ser alterados.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// Novo nome do usuário.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Novo telefone do usuário.
        /// </summary>
        public string Telefone { get; set; } = null!;

        /// <summary>
        /// Tipo de usuário (Admin, Capitao, Jogador).
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }
    }
}
