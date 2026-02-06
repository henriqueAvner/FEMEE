using FEMEE.Application.DTOs.Auth;

namespace FEMEE.Application.Interfaces.Services
{
    /// <summary>
    /// Interface para o serviço de autenticação.
    /// Define operações de login, registro e gerenciamento de senha.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza login do usuário.
        /// Valida email e senha, gera token JWT.
        /// </summary>
        /// <param name="request">Dados de login</param>
        /// <returns>Resposta com token e dados do usuário</returns>
        /// <exception cref="UnauthorizedAccessException">Se email ou senha inválidos</exception>
        Task<LoginResponse> LoginAsync(LoginRequest request);

        /// <summary>
        /// Registra um novo usuário.
        /// Valida dados, faz hash da senha, cria usuário.
        /// </summary>
        /// <param name="request">Dados de registro</param>
        /// <returns>Resposta com token e dados do novo usuário</returns>
        /// <exception cref="InvalidOperationException">Se email já existe ou dados inválidos</exception>
        Task<LoginResponse> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Atualiza a senha de um usuário.
        /// Valida senha atual antes de atualizar.
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <param name="senhaAtual">Senha atual em texto plano</param>
        /// <param name="novaSenha">Nova senha em texto plano</param>
        /// <exception cref="UnauthorizedAccessException">Se senha atual inválida</exception>
        Task ChangePasswordAsync(int userId, string senhaAtual, string novaSenha);
    }
}
