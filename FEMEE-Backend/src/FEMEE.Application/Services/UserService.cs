using AutoMapper;
using FEMEE.Application.DTOs.User;
using FEMEE.Application.Interfaces.Common;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Interfaces.Services;
using FEMEE.Application.Logging;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Serviço de usuários.
    /// Implementa as operações de CRUD e lógica de negócio para usuários.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Construtor do serviço de usuários.
        /// </summary>
        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        /// <summary>
        /// Obtém um usuário pelo ID.
        /// </summary>
        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("GetUserById", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando usuário com ID: {UserId}", id);

                    var user = await _unitOfWork.Users.GetByIdAsync(id);

                    if (user == null)
                    {
                        _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                        throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Usuário encontrado em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<UserResponseDto>(user);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar usuário em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtém todos os usuários.
        /// </summary>
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            using (StructuredLogging.BeginOperationScope("GetAllUsers"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Buscando todos os usuários");

                    var users = await _unitOfWork.Users.GetAllAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Total de usuários encontrados: {Count} em {ElapsedMilliseconds}ms",
                        users.Count(), stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<IEnumerable<UserResponseDto>>(users);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar todos os usuários em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtém um usuário pelo email.
        /// </summary>
        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            using (StructuredLogging.BeginOperationScope("GetUserByEmail"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        _logger.LogWarning("Email vazio fornecido");
                        throw new ArgumentException("Email não pode ser vazio", nameof(email));
                    }

                    _logger.LogInformation("Buscando usuário por email: {Email}", email);

                    var user = await _unitOfWork.Users.GetByEmailAsync(email);

                    if (user == null)
                    {
                        _logger.LogWarning("Usuário não encontrado: {Email}", email);
                        throw new KeyNotFoundException($"Usuário com email {email} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Usuário encontrado por email em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<UserResponseDto>(user);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar usuário por email em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obtém a entidade de domínio `User` pelo email.
        /// Usado internamente por serviços que necessitam da entidade completa (ex.: autenticação).
        /// </summary>
        public async Task<User> GetUserEntityByEmailAsync(string email)
        {
            using (StructuredLogging.BeginOperationScope("GetUserEntityByEmail"))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        _logger.LogWarning("Email vazio fornecido");
                        throw new ArgumentException("Email não pode ser vazio", nameof(email));
                    }

                    _logger.LogInformation("Buscando entidade de usuário por email: {Email}", email);

                    var user = await _unitOfWork.Users.GetByEmailAsync(email);

                    if (user == null)
                    {
                        _logger.LogWarning("Usuário não encontrado: {Email}", email);
                        throw new KeyNotFoundException($"Usuário com email {email} não encontrado");
                    }

                    stopwatch.Stop();
                    _logger.LogInformation("Entidade de usuário encontrada por email em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);

                    return user;
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao buscar entidade de usuário por email em {ElapsedMilliseconds}ms", stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto)
        {
            using (StructuredLogging.BeginOperationScope("CreateUser"))
            {
                StructuredLogging.AddEntityContext("User", 0);

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Iniciando criação de usuário: {Email}", dto.Email);

                    // Verificar se email já existe
                    if (await _unitOfWork.Users.EmailExistsAsync(dto.Email))
                    {
                        _logger.LogWarning("Email já cadastrado: {Email}", dto.Email);
                        throw new InvalidOperationException("Email já cadastrado");
                    }

                    var user = _mapper.Map<User>(dto);
                    user.Senha = _passwordHasher.HashPassword(dto.Senha);
                    user.DataCriacao = DateTime.UtcNow;
                    user.DataAtualizacao = DateTime.UtcNow;

                    _logger.LogDebug("Usuário mapeado e senha hasheada");

                    await _unitOfWork.Users.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Usuário criado com sucesso em {ElapsedMilliseconds}ms: {UserId}",
                        stopwatch.ElapsedMilliseconds, user.Id);

                    return _mapper.Map<UserResponseDto>(user);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao criar usuário em {ElapsedMilliseconds}ms: {Email}",
                        stopwatch.ElapsedMilliseconds, dto.Email);
                    throw;
                }
            }
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        public async Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserDto dto)
        {
            using (StructuredLogging.BeginOperationScope("UpdateUser", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Atualizando usuário: {UserId}", id);

                    var user = await _unitOfWork.Users.GetByIdAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                        throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");
                    }

                    _mapper.Map(dto, user);
                    user.DataAtualizacao = DateTime.UtcNow;

                    await _unitOfWork.Users.UpdateAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Usuário atualizado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);

                    return _mapper.Map<UserResponseDto>(user);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao atualizar usuário em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deleta um usuário.
        /// </summary>
        public async Task DeleteUserAsync(int id)
        {
            using (StructuredLogging.BeginOperationScope("DeleteUser", id))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    _logger.LogInformation("Deletando usuário: {UserId}", id);

                    var user = await _unitOfWork.Users.GetByIdAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning("Usuário não encontrado: {UserId}", id);
                        throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");
                    }

                    await _unitOfWork.Users.DeleteAsync(id);
                    await _unitOfWork.SaveChangesAsync();

                    stopwatch.Stop();
                    _logger.LogInformation("Usuário deletado com sucesso em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "Erro ao deletar usuário em {ElapsedMilliseconds}ms",
                        stopwatch.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}
