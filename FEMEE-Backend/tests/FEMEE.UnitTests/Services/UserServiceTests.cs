using AutoMapper;
using FEMEE.Application.DTOs.User;
using FEMEE.Application.Interfaces.Common;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Application.Mappings;
using FEMEE.Application.Services;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace FEMEE.UnitTests.Services;

/// <summary>
/// Testes unitários para UserService.
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _sut; // System Under Test

    public UserServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _loggerMock = new Mock<ILogger<UserService>>();

        // Configura o UnitOfWork para retornar o mock do repositório
        _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);

        // Configura o PasswordHasher
        _passwordHasherMock.Setup(p => p.HashPassword(It.IsAny<string>())).Returns("hashedpassword");

        // Configura o AutoMapper real
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _sut = new UserService(_unitOfWorkMock.Object, _mapper, _loggerMock.Object, _passwordHasherMock.Object);
    }

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var user = CreateSampleUser(userId);
        _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal(user.Nome, result.Nome);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidId = 999;
        _userRepositoryMock.Setup(r => r.GetByIdAsync(invalidId)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetUserByIdAsync(invalidId));
    }

    #endregion

    #region GetByEmailAsync Tests

    [Fact]
    public async Task GetByEmailAsync_WithValidEmail_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var user = CreateSampleUser(1, email);
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_WithInvalidEmail_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidEmail = "notfound@example.com";
        _userRepositoryMock.Setup(r => r.GetByEmailAsync(invalidEmail)).ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetUserByEmailAsync(invalidEmail));
    }

    #endregion

    #region CreateUserAsync Tests

    [Fact]
    public async Task CreateUserAsync_WithValidData_ReturnsNewUser()
    {
        // Arrange
        var createDto = new CreateUserDto
        {
            Nome = "Novo Usuário",
            Email = "novo@example.com",
            Telefone = "11999999999",
            Senha = "SenhaSegura123!"
        };

        _userRepositoryMock.Setup(r => r.EmailExistsAsync(createDto.Email)).ReturnsAsync(false);
        _userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync((User u) => u);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _sut.CreateUserAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createDto.Nome, result.Nome);
        Assert.Equal(createDto.Email, result.Email);
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var createDto = new CreateUserDto
        {
            Nome = "Usuário Duplicado",
            Email = "duplicado@example.com",
            Telefone = "11999999999",
            Senha = "SenhaSegura123!"
        };

        _userRepositoryMock.Setup(r => r.EmailExistsAsync(createDto.Email)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateUserAsync(createDto));
        _userRepositoryMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    #endregion

    #region GetAllUsersAsync Tests

    [Fact]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            CreateSampleUser(1, "user1@example.com"),
            CreateSampleUser(2, "user2@example.com"),
            CreateSampleUser(3, "user3@example.com")
        };
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllUsersAsync_WithNoUsers_ReturnsEmptyList()
    {
        // Arrange
        _userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User>());

        // Act
        var result = await _sut.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region Helper Methods

    private static User CreateSampleUser(int id, string email = "test@example.com")
    {
        return new User
        {
            Id = id,
            Nome = $"Usuário {id}",
            Email = email,
            Senha = "hashedpassword",
            Telefone = "11999999999",
            TipoUsuario = TipoUsuario.Visitante,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };
    }

    #endregion
}
