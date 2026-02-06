using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para GenericRepository.
/// Usa User como entidade de teste.
/// </summary>
public class GenericRepositoryTests : RepositoryTestBase
{
    private readonly GenericRepository<User> _repository;

    public GenericRepositoryTests()
    {
        _repository = new GenericRepository<User>(Context);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        // Arrange
        Context.Users.AddRange(
            CreateUser("user1@test.com"),
            CreateUser("user2@test.com"),
            CreateUser("user3@test.com")
        );
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_WithNoEntities_ReturnsEmpty()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsEntity()
    {
        // Arrange
        var user = CreateUser("test@test.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region FindAsync Tests

    [Fact]
    public async Task FindAsync_WithMatchingPredicate_ReturnsFilteredEntities()
    {
        // Arrange
        Context.Users.AddRange(
            CreateUser("admin@test.com", TipoUsuario.Administrador),
            CreateUser("user1@test.com", TipoUsuario.Visitante),
            CreateUser("user2@test.com", TipoUsuario.Visitante)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.FindAsync(u => u.TipoUsuario == TipoUsuario.Visitante)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, u => Assert.Equal(TipoUsuario.Visitante, u.TipoUsuario));
    }

    [Fact]
    public async Task FindAsync_WithNoMatches_ReturnsEmpty()
    {
        // Arrange
        Context.Users.Add(CreateUser("user@test.com", TipoUsuario.Visitante));
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.FindAsync(u => u.TipoUsuario == TipoUsuario.Administrador)).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task FindAsync_WithComplexPredicate_ReturnsCorrectEntities()
    {
        // Arrange
        Context.Users.AddRange(
            CreateUser("admin1@test.com", TipoUsuario.Administrador),
            CreateUser("admin2@test.com", TipoUsuario.Administrador),
            CreateUser("user@test.com", TipoUsuario.Visitante)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.FindAsync(u => 
            u.TipoUsuario == TipoUsuario.Administrador && 
            u.Email!.Contains("1"))).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("admin1@test.com", result[0].Email);
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_AddsEntityToDatabase()
    {
        // Arrange
        var user = CreateUser("new@test.com");

        // Act
        var result = await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(result.Id > 0);
        Assert.Single(Context.Users);
    }

    [Fact]
    public async Task AddAsync_WithNullEntity_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.AddAsync(null!));
    }

    [Fact]
    public async Task AddAsync_ReturnsAddedEntity()
    {
        // Arrange
        var user = CreateUser("test@test.com");

        // Act
        var result = await _repository.AddAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Same(user, result);
        Assert.True(result.Id > 0);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_UpdatesEntityInDatabase()
    {
        // Arrange
        var user = CreateUser("original@test.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        
        // Act
        user.Nome = "Updated Name";
        user.Email = "updated@test.com";
        await _repository.UpdateAsync(user);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Users.FindAsync(user.Id);
        Assert.Equal("Updated Name", updated!.Nome);
        Assert.Equal("updated@test.com", updated.Email);
    }

    [Fact]
    public async Task UpdateAsync_WithNullEntity_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.UpdateAsync(null!));
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_RemovesEntityFromDatabase()
    {
        // Arrange
        var user = CreateUser("delete@test.com");
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        var userId = user.Id;

        // Act
        await _repository.DeleteAsync(userId);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Null(await Context.Users.FindAsync(userId));
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingId_DoesNotThrow()
    {
        // Act & Assert - Should not throw
        await _repository.DeleteAsync(999);
        await Context.SaveChangesAsync();
    }

    #endregion

    #region GetPagedAsync Tests

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        // Arrange
        for (int i = 1; i <= 25; i++)
        {
            Context.Users.Add(CreateUser($"user{i}@test.com"));
        }
        await Context.SaveChangesAsync();

        // Act
        var page1 = (await _repository.GetPagedAsync(1, 10)).ToList();
        var page2 = (await _repository.GetPagedAsync(2, 10)).ToList();
        var page3 = (await _repository.GetPagedAsync(3, 10)).ToList();

        // Assert
        Assert.Equal(10, page1.Count);
        Assert.Equal(10, page2.Count);
        Assert.Equal(5, page3.Count);
    }

    [Fact]
    public async Task GetPagedAsync_WithInvalidPageNumber_DefaultsToPage1()
    {
        // Arrange
        Context.Users.AddRange(
            CreateUser("user1@test.com"),
            CreateUser("user2@test.com")
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetPagedAsync(-1, 10)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPagedAsync_WithInvalidPageSize_DefaultsTo10()
    {
        // Arrange
        for (int i = 1; i <= 15; i++)
        {
            Context.Users.Add(CreateUser($"user{i}@test.com"));
        }
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetPagedAsync(1, -5)).ToList();

        // Assert
        Assert.Equal(10, result.Count);
    }

    [Fact]
    public async Task GetPagedAsync_WithPageSizeOver100_LimitsTo100()
    {
        // Arrange
        for (int i = 1; i <= 150; i++)
        {
            Context.Users.Add(CreateUser($"user{i}@test.com"));
        }
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetPagedAsync(1, 200)).ToList();

        // Assert
        Assert.Equal(100, result.Count);
    }

    #endregion

    #region Helper Methods

    private static User CreateUser(string email, TipoUsuario tipo = TipoUsuario.Visitante)
    {
        return new User
        {
            Nome = "Test User",
            Email = email,
            Senha = "hashedpassword",
            Telefone = "11999999999",
            TipoUsuario = tipo,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };
    }

    #endregion
}
