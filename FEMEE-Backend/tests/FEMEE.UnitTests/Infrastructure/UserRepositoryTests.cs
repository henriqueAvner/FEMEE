using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para UserRepository.
/// </summary>
public class UserRepositoryTests : RepositoryTestBase
{
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository(Context);
    }

    #region GetByEmailAsync Tests

    [Fact]
    public async Task GetByEmailAsync_WithExistingEmail_ReturnsUser()
    {
        // Arrange
        var user = CreateUser("test@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_WithNonExistingEmail_ReturnsNull()
    {
        // Arrange
        var user = CreateUser("existing@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("notfound@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_IsCaseInsensitive()
    {
        // Arrange
        var user = CreateUser("Test@Example.COM");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetByEmailAsync_WithInvalidEmail_ReturnsNull(string? email)
    {
        // Act
        var result = await _repository.GetByEmailAsync(email!);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region EmailExistsAsync Tests

    [Fact]
    public async Task EmailExistsAsync_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        var user = CreateUser("exists@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.EmailExistsAsync("exists@example.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EmailExistsAsync_WithNonExistingEmail_ReturnsFalse()
    {
        // Arrange
        var user = CreateUser("exists@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.EmailExistsAsync("notexists@example.com");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EmailExistsAsync_IsCaseInsensitive()
    {
        // Arrange
        var user = CreateUser("Test@EXAMPLE.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.EmailExistsAsync("TEST@example.COM");

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task EmailExistsAsync_WithInvalidEmail_ReturnsFalse(string? email)
    {
        // Act
        var result = await _repository.EmailExistsAsync(email!);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region GenericRepository Methods Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        Context.Users.AddRange(
            CreateUser("user1@example.com"),
            CreateUser("user2@example.com"),
            CreateUser("user3@example.com")
        );
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = CreateUser("test@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsUserToDatabase()
    {
        // Arrange
        var user = CreateUser("new@example.com");

        // Act
        var result = await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(result.Id > 0);
        Assert.Equal(1, Context.Users.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesUserInDatabase()
    {
        // Arrange
        var user = CreateUser("original@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        user.Nome = "Updated Name";
        await _repository.UpdateAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Users.FindAsync(user.Id);
        Assert.Equal("Updated Name", updated!.Nome);
    }

    [Fact]
    public async Task DeleteAsync_RemovesUserFromDatabase()
    {
        // Arrange
        var user = CreateUser("delete@example.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        await _repository.DeleteAsync(userId);
        await Context.SaveChangesAsync();

        // Assert
        var deleted = await Context.Users.FindAsync(userId);
        Assert.Null(deleted);
    }

    #endregion

    #region Helper Methods

    private static User CreateUser(string email)
    {
        return new User
        {
            Nome = "Test User",
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
