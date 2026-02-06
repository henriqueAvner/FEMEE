using FEMEE.Domain.Entities.Principal;

namespace FEMEE.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface especializada para repositório de usuários.
    /// Estende IRepository com métodos específicos de User.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Obtém um usuário pelo email.
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Usuário encontrado ou null</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Verifica se um email já está cadastrado.
        /// </summary>
        /// <param name="email">Email a verificar</param>
        /// <returns>True se email já existe</returns>
        Task<bool> EmailExistsAsync(string email);
    }
}
