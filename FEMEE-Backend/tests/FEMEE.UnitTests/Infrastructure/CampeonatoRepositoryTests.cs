using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para CampeonatoRepository.
/// </summary>
public class CampeonatoRepositoryTests : RepositoryTestBase
{
    private readonly CampeonatoRepository _repository;

    public CampeonatoRepositoryTests()
    {
        _repository = new CampeonatoRepository(Context);
    }

    #region GetCampeonatosAtivosAsync Tests

    [Fact]
    public async Task GetCampeonatosAtivosAsync_ReturnsOnlyActiveChampionships()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        Context.Campeonatos.AddRange(
            CreateCampeonato("Ativo 1", jogo.Id, StatusCampeonato.InProgress),
            CreateCampeonato("Ativo 2", jogo.Id, StatusCampeonato.Open),
            CreateCampeonato("Finalizado", jogo.Id, StatusCampeonato.Closed),
            CreateCampeonato("Cancelado", jogo.Id, StatusCampeonato.Cancelled)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosAtivosAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => 
            Assert.True(c.Status == StatusCampeonato.InProgress || c.Status == StatusCampeonato.Open));
    }

    [Fact]
    public async Task GetCampeonatosAtivosAsync_WithNoActiveChampionships_ReturnsEmpty()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        Context.Campeonatos.Add(CreateCampeonato("Finalizado", jogo.Id, StatusCampeonato.Closed));
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosAtivosAsync()).ToList();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region GetCampeonatosComInscricoesAbertasAsync Tests

    [Fact]
    public async Task GetCampeonatosComInscricoesAbertasAsync_ReturnsOnlyOpenChampionships()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        Context.Campeonatos.AddRange(
            CreateCampeonato("Open 1", jogo.Id, StatusCampeonato.Open),
            CreateCampeonato("Open 2", jogo.Id, StatusCampeonato.Open),
            CreateCampeonato("In Progress", jogo.Id, StatusCampeonato.InProgress)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosComInscricoesAbertasAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(StatusCampeonato.Open, c.Status));
    }

    #endregion

    #region GetCampeonatosByJogoAsync Tests

    [Fact]
    public async Task GetCampeonatosByJogoAsync_ReturnsChampionshipsForGame()
    {
        // Arrange
        var jogo1 = await CreateAndSaveJogo("League of Legends");
        var jogo2 = await CreateAndSaveJogo("Counter-Strike");

        Context.Campeonatos.AddRange(
            CreateCampeonato("LoL Cup 1", jogo1.Id),
            CreateCampeonato("LoL Cup 2", jogo1.Id),
            CreateCampeonato("CS Cup", jogo2.Id)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosByJogoAsync(jogo1.Id)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal(jogo1.Id, c.JogoId));
    }

    #endregion

    #region IsTimeInscritoAsync Tests

    [Fact]
    public async Task IsTimeInscritoAsync_WhenTeamIsRegistered_ReturnsTrue()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var time = await CreateAndSaveTime();
        var campeonato = CreateCampeonato("Test Cup", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();

        var inscricao = new InscricaoCampeonato
        {
            CampeonatoId = campeonato.Id,
            TimeId = time.Id,
            CapitaoId = 1,
            DataInscricao = DateTime.UtcNow,
            StatusInscricao = StatusInscricao.Aprovada,
            TelefoneContato = "11999999999",
            EmailContato = "test@test.com",
            NomeCapitao = "Capitao",
            NomeTime = "Time"
        };
        Context.InscricoesCampeonatos.Add(inscricao);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.IsTimeInscritoAsync(campeonato.Id, time.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsTimeInscritoAsync_WhenTeamIsNotRegistered_ReturnsFalse()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var time = await CreateAndSaveTime();
        var campeonato = CreateCampeonato("Test Cup", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.IsTimeInscritoAsync(campeonato.Id, time.Id);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region GetCampeonatosComPartidasAsync Tests

    [Fact]
    public async Task GetCampeonatosComPartidasAsync_IncludesPartidas()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var campeonato = CreateCampeonato("Test Cup", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosComPartidasAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.NotNull(result[0].Partidas);
    }

    #endregion

    #region GetCampeonatosComTimesAsync Tests

    [Fact]
    public async Task GetCampeonatosComTimesAsync_IncludesInscricoes()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var campeonato = CreateCampeonato("Test Cup", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetCampeonatosComTimesAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.NotNull(result[0].InscricoesCampeonatos);
    }

    #endregion

    #region Generic Repository Tests

    [Fact]
    public async Task AddAsync_AddsCampeonatoToDatabase()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var campeonato = CreateCampeonato("New Cup", jogo.Id);

        // Act
        await _repository.AddAsync(campeonato);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(campeonato.Id > 0);
        Assert.Single(Context.Campeonatos);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsCampeonato()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var campeonato = CreateCampeonato("Test Cup", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(campeonato.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(campeonato.Id, result.Id);
    }

    [Fact]
    public async Task DeleteAsync_RemovesCampeonatoFromDatabase()
    {
        // Arrange
        var jogo = await CreateAndSaveJogo();
        var campeonato = CreateCampeonato("To Delete", jogo.Id);
        Context.Campeonatos.Add(campeonato);
        await Context.SaveChangesAsync();
        var campeonatoId = campeonato.Id;

        // Act
        await _repository.DeleteAsync(campeonatoId);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Null(await Context.Campeonatos.FindAsync(campeonatoId));
    }

    #endregion

    #region Helper Methods

    private async Task<Jogo> CreateAndSaveJogo(string nome = "Test Game")
    {
        var jogo = new Jogo
        {
            Nome = nome,
            Slug = nome.ToLower().Replace(" ", "-"),
            CategoriaJogo = CategoriaJogo.LeagueOfLegends
        };
        Context.Jogos.Add(jogo);
        await Context.SaveChangesAsync();
        return jogo;
    }

    private async Task<Time> CreateAndSaveTime(string nome = "Test Team")
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

    private static Campeonato CreateCampeonato(
        string titulo,
        int jogoId,
        StatusCampeonato status = StatusCampeonato.Open)
    {
        return new Campeonato
        {
            Titulo = titulo,
            JogoId = jogoId,
            Status = status,
            DataInicio = DateTime.UtcNow.AddDays(7),
            DataFim = DateTime.UtcNow.AddDays(30),
            DataLimiteInscricao = DateTime.UtcNow.AddDays(5),
            Local = "Online",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    #endregion
}
