using FEMEE.Application.DTOs.User;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de usuários.
    /// Define as operações disponíveis para gerenciar usuários.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>DTO do usuário</returns>
        /// <exception cref="KeyNotFoundException">Quando usuário não existe</exception>
        Task<UserResponseDto> GetUserByIdAsync(int id);

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        /// <returns>Coleção de DTOs de usuários</returns>
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        /// <summary>
        /// Obtém um usuário pelo email.
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>DTO do usuário</returns>
        /// <exception cref="KeyNotFoundException">Quando usuário não existe</exception>
        Task<UserResponseDto> GetUserByEmailAsync(string email);

        /// <summary>
        /// Obtém a entidade de domínio `User` pelo email (usado internamente para autenticação).
        /// </summary>
        /// <param name="email">Email do usuário</param>
        /// <returns>Entidade de domínio User</returns>
        Task<FEMEE.Domain.Entities.Principal.User> GetUserEntityByEmailAsync(string email);

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="dto">DTO de criação de usuário</param>
        /// <returns>DTO do usuário criado</returns>
        /// <exception cref="InvalidOperationException">Quando email já existe</exception>
        Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <param name="dto">DTO de atualização</param>
        /// <returns>DTO do usuário atualizado</returns>
        /// <exception cref="KeyNotFoundException">Quando usuário não existe</exception>
        Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto);

        /// <summary>
        /// Deleta um usuário.
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <exception cref="KeyNotFoundException">Quando usuário não existe</exception>
        Task DeleteUserAsync(int id);
    }
}
