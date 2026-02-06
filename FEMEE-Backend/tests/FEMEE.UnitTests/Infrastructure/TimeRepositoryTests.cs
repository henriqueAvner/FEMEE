using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para TimeRepository.
/// </summary>
public class TimeRepositoryTests : RepositoryTestBase
{
    private readonly TimeRepository _repository;

    public TimeRepositoryTests()
    {
        _repository = new TimeRepository(Context);
    }

    #region GetBySlugAsync Tests

    [Fact]
    public async Task GetBySlugAsync_WithExistingSlug_ReturnsTime()
    {
        // Arrange
        var time = CreateTime("Team Alpha", "team-alpha");
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBySlugAsync("team-alpha");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("team-alpha", result.Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_WithNonExistingSlug_ReturnsNull()
    {
        // Arrange
        var time = CreateTime("Team Alpha", "team-alpha");
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetBySlugAsync("non-existing");

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region GetRankingAsync Tests

    [Fact]
    public async Task GetRankingAsync_ReturnsTeamsOrderedByPontos()
    {
        // Arrange
        Context.Times.AddRange(
            CreateTime("Team C", "team-c", pontos: 10),
            CreateTime("Team A", "team-a", pontos: 30),
            CreateTime("Team B", "team-b", pontos: 20)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetRankingAsync(10)).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Team A", result[0].Nome); // 30 pontos
        Assert.Equal("Team B", result[1].Nome); // 20 pontos
        Assert.Equal("Team C", result[2].Nome); // 10 pontos
    }

    [Fact]
    public async Task GetRankingAsync_WithTopLimit_ReturnsLimitedResults()
    {
        // Arrange
        Context.Times.AddRange(
            CreateTime("Team 1", "team-1", pontos: 50),
            CreateTime("Team 2", "team-2", pontos: 40),
            CreateTime("Team 3", "team-3", pontos: 30),
            CreateTime("Team 4", "team-4", pontos: 20),
            CreateTime("Team 5", "team-5", pontos: 10)
        );
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetRankingAsync(3)).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("Team 1", result[0].Nome);
    }

    #endregion

    #region AtualizarVitoriasAsync Tests

    [Fact]
    public async Task AtualizarVitoriasAsync_IncrementsVitoriasAndPontos()
    {
        // Arrange
        var time = CreateTime("Test Team", "test-team", vitorias: 5, pontos: 15);
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        await _repository.AtualizarVitoriasAsync(time.Id, 3);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Times.FindAsync(time.Id);
        Assert.Equal(6, updated!.Vitorias);
        Assert.Equal(18, updated.Pontos);
    }

    [Fact]
    public async Task AtualizarVitoriasAsync_WithNonExistingId_DoesNotThrow()
    {
        // Act & Assert - should not throw
        await _repository.AtualizarVitoriasAsync(999, 3);
    }

    #endregion

    #region AtualizarDerrotasAsync Tests

    [Fact]
    public async Task AtualizarDerrotasAsync_IncrementsDerrotasAndReducesPontos()
    {
        // Arrange
        var time = CreateTime("Test Team", "test-team", derrotas: 2, pontos: 10);
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        await _repository.AtualizarDerrotasAsync(time.Id, -3);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Times.FindAsync(time.Id);
        Assert.Equal(3, updated!.Derrotas);
        Assert.Equal(7, updated.Pontos);
    }

    [Fact]
    public async Task AtualizarDerrotasAsync_PontosNeverGoBelowZero()
    {
        // Arrange
        var time = CreateTime("Test Team", "test-team", derrotas: 0, pontos: 2);
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        await _repository.AtualizarDerrotasAsync(time.Id, -5);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Times.FindAsync(time.Id);
        Assert.Equal(0, updated!.Pontos); // Math.Max(0, 2 + (-5)) = 0
    }

    #endregion

    #region AtualizarEmpatesAsync Tests

    [Fact]
    public async Task AtualizarEmpatesAsync_IncrementsEmpatesAndPontos()
    {
        // Arrange
        var time = CreateTime("Test Team", "test-team", empates: 3, pontos: 12);
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        await _repository.AtualizarEmpatesAsync(time.Id, 1);
        await Context.SaveChangesAsync();

        // Assert
        var updated = await Context.Times.FindAsync(time.Id);
        Assert.Equal(4, updated!.Empates);
        Assert.Equal(13, updated.Pontos);
    }

    #endregion

    #region GetTimesWithPlayersAsync Tests

    [Fact]
    public async Task GetTimesWithPlayersAsync_ReturnsTimesWithJogadores()
    {
        // Arrange
        var time = CreateTime("Test Team", "test-team");
        Context.Times.Add(time);
        await Context.SaveChangesAsync();

        // Act
        var result = (await _repository.GetTimesWithPlayersAsync()).ToList();

        // Assert
        Assert.Single(result);
        Assert.NotNull(result[0].Jogadores);
    }

    #endregion

    #region Generic Repository Tests

    [Fact]
    public async Task AddAsync_AddsTimeToDatabase()
    {
        // Arrange
        var time = CreateTime("New Team", "new-team");

        // Act
        await _repository.AddAsync(time);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(time.Id > 0);
        Assert.Single(Context.Times);
    }

    [Fact]
    public async Task DeleteAsync_RemovesTimeFromDatabase()
    {
        // Arrange
        var time = CreateTime("To Delete", "to-delete");
        Context.Times.Add(time);
        await Context.SaveChangesAsync();
        var timeId = time.Id;

        // Act
        await _repository.DeleteAsync(timeId);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Null(await Context.Times.FindAsync(timeId));
    }

    #endregion

    #region Helper Methods

    private static Time CreateTime(
        string nome, 
        string slug, 
        int vitorias = 0, 
        int derrotas = 0, 
        int empates = 0, 
        int pontos = 0)
    {
        return new Time
        {
            Nome = nome,
            Slug = slug,
            Vitorias = vitorias,
            Derrotas = derrotas,
            Empates = empates,
            Pontos = pontos,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}
