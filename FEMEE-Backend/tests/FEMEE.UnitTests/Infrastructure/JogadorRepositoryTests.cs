using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para JogadorRepository.
/// </summary>
public class JogadorRepositoryTests : RepositoryTestBase
{
    private readonly JogadorRepository _repository;

    public JogadorRepositoryTests()
    {
        _repository = new JogadorRepository(Context);
    }

    #region GetByNickNameAsync Tests

    [Fact]
    public async Task GetByNickNameAsync_WithExistingNickname_ReturnsJogador()
    {
        // Arrange
        var user = await CreateAndSaveUser();
        var jogador = CreateJogador(user.Id, "ProPlayer123");
        Context.Jogadores.Add(jogador);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByNickNameAsync("ProPlayer123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ProPlayer123", result.NickName);
    }

    [Fact]
    public async Task GetByNickNameAsync_WithNonExistingNickname_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _repository.GetByNickNameAsync("NotExists"));
    }

    #endregion

    #region GetByFuncaoAsync Tests

    [Fact]
    public async Task GetByFuncaoAsync_ReturnsJogadoresWithMatchingFuncao()
    {
        // Arrange
        var user1 = await CreateAndSaveUser("user1@test.com");
        var user2 = await CreateAndSaveUser("user2@test.com");
        var user3 = await CreateAndSaveUser("user3@test.com");

        Context.Jogadores.AddRange(
            CreateJogador(user1.Id, "Player1", FuncaoJogador.Capitao),
            CreateJogador(user2.Id, "Player2", FuncaoJogador.Jogador),
            CreateJogador(user3.Id, "Player3", FuncaoJogador.Capitao)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetByFuncaoAsync(FuncaoJogador.Capitao)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, j => Assert.Equal(FuncaoJogador.Capitao, j.Funcao));
    }

    [Fact]
    public async Task GetByFuncaoAsync_WithNoMatches_ReturnsEmpty()
    {
        // Arrange
        var user = await CreateAndSaveUser();
        Context.Jogadores.Add(CreateJogador(user.Id, "Player1", FuncaoJogador.Jogador));
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetByFuncaoAsync(FuncaoJogador.Capitao)).ToList();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetJogadoresByTimeIdAsync Tests

    [Fact]
    public async Task GetJogadoresByTimeIdAsync_ReturnsJogadoresFromTeam()
    {
        // Arrange
        var time = await CreateAndSaveTime("Test Team");
        var user1 = await CreateAndSaveUser("user1@test.com");
        var user2 = await CreateAndSaveUser("user2@test.com");
        var user3 = await CreateAndSaveUser("user3@test.com");

        Context.Jogadores.AddRange(
            CreateJogador(user1.Id, "Player1", timeId: time.Id),
            CreateJogador(user2.Id, "Player2", timeId: time.Id),
            CreateJogador(user3.Id, "Player3", timeId: null) // Sem time
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetJogadoresByTimeIdAsync(time.Id)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, j => Assert.Equal(time.Id, j.TimeId));
    }

    #endregion

    #region GetJogadoresAtivosAsync Tests

    [Fact]
    public async Task GetJogadoresAtivosAsync_ReturnsOnlyActivePlayersFromTeam()
    {
        // Arrange
        var time = await CreateAndSaveTime("Test Team");
        var user1 = await CreateAndSaveUser("user1@test.com");
        var user2 = await CreateAndSaveUser("user2@test.com");
        var user3 = await CreateAndSaveUser("user3@test.com");

        Context.Jogadores.AddRange(
            CreateJogador(user1.Id, "Active1", timeId: time.Id, status: StatusJogador.Ativo),
            CreateJogador(user2.Id, "Inactive", timeId: time.Id, status: StatusJogador.Inativo),
            CreateJogador(user3.Id, "Active2", timeId: time.Id, status: StatusJogador.Ativo)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetJogadoresAtivosAsync(time.Id)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, j => Assert.Equal(StatusJogador.Ativo, j.Status));
    }

    [Fact]
    public async Task GetJogadoresAtivosAsync_WithNoActivePlayers_ReturnsEmpty()
    {
        // Arrange
        var time = await CreateAndSaveTime("Test Team");
        var user = await CreateAndSaveUser();
        Context.Jogadores.Add(
            CreateJogador(user.Id, "Inactive", timeId: time.Id, status: StatusJogador.Inativo)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetJogadoresAtivosAsync(time.Id)).ToList();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetWithUserAsync Tests

    [Fact]
    public async Task GetWithUserAsync_WithExistingId_ReturnsJogador()
    {
        // Arrange
        var user = await CreateAndSaveUser();
        var jogador = CreateJogador(user.Id, "TestPlayer");
        Context.Jogadores.Add(jogador);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetWithUserAsync(jogador.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(jogador.Id, result.Id);
    }

    [Fact]
    public async Task GetWithUserAsync_WithNonExistingId_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _repository.GetWithUserAsync(999));
    }

    #endregion

    #region Generic Repository Tests

    [Fact]
    public async Task AddAsync_AddsJogadorToDatabase()
    {
        // Arrange
        var user = await CreateAndSaveUser();
        var jogador = CreateJogador(user.Id, "NewPlayer");

        // Act
        await _repository.AddAsync(jogador);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(jogador.Id > 0);
        Assert.Single(Context.Jogadores);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllJogadores()
    {
        // Arrange
        var user1 = await CreateAndSaveUser("user1@test.com");
        var user2 = await CreateAndSaveUser("user2@test.com");
        
        Context.Jogadores.AddRange(
            CreateJogador(user1.Id, "Player1"),
            CreateJogador(user2.Id, "Player2")
        );
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteAsync_RemovesJogadorFromDatabase()
    {
        // Arrange
        var user = await CreateAndSaveUser();
        var jogador = CreateJogador(user.Id, "ToDelete");
        Context.Jogadores.Add(jogador);
        await Context.SaveChangesAsync();
        var jogadorId = jogador.Id;

        // Act
        await _repository.DeleteAsync(jogadorId);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Null(await Context.Jogadores.FindAsync(jogadorId));
    }

    #endregion

    #region Helper Methods

    private async Task<User> CreateAndSaveUser(string email = "test@example.com")
    {
        var user = new User
        {
            Nome = "Test User",
            Email = email,
            Senha = "hashedpassword",
            Telefone = "11999999999",
            TipoUsuario = TipoUsuario.Jogador,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }

    private async Task<Time> CreateAndSaveTime(string nome)
    {
        var time = new Time
        {
            Nome = nome,
            Slug = nome.ToLower().Replace(" ", "-"),
            CreatedAt = DateTime.UtcNow
        };
        Context.Times.Add(time);
        await Context.SaveChangesAsync();
        return time;
    }

    private static Jogador CreateJogador(
        int userId,
        string nickname,
        FuncaoJogador funcao = FuncaoJogador.Jogador,
        int? timeId = null,
        StatusJogador status = StatusJogador.Ativo)
    {
        return new Jogador
        {
            UserId = userId,
            NickName = nickname,
            Funcao = funcao,
            TimeId = timeId,
            Status = status,
            DataEntradaTime = DateTime.UtcNow
        };
    }

    #endregion
}
