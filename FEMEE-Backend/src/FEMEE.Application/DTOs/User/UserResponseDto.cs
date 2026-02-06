using FEMEE.Domain.Enums;

namespace FEMEE.Application.DTOs.User
{
    /// <summary>
    /// DTO para resposta de usuário.
    /// Contém apenas os dados que devem ser retornados.
    /// IMPORTANTE: Não inclui SenhaHash!
    /// </summary>
    public class UserResponseDto
    {
        /// <summary>
        /// ID único do usuário.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome completo do usuário.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Email do usuário.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Telefone do usuário.
        /// </summary>
        public string Telefone { get; set; } = null!;

        /// <summary>
        /// Tipo de usuário.
        /// </summary>
        public TipoUsuario TipoUsuario { get; set; }

        /// <summary>
        /// Data de criação do usuário.
        /// </summary>
        public DateTime DataCriacao { get; set; }
    }
}
