using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;
using FEMEE.Infrastructure.Data;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Testes para o UnitOfWork.
/// </summary>
public class UnitOfWorkTests : IDisposable
{
    private readonly FemeeDbContext _context;
    private readonly UnitOfWork _unitOfWork;
    private static int _userIdCounter = 1000;
    private static int _timeIdCounter = 1000;
    private static int _partidaIdCounter = 1000;

    public UnitOfWorkTests()
    {
        var options = new DbContextOptionsBuilder<FemeeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new FemeeDbContext(options);
        _context.Database.EnsureCreated();
        _unitOfWork = new UnitOfWork(_context);
    }

    public void Dispose()
    {
        // Não dispor o UnitOfWork aqui pois ele já dispõe o contexto
        // Apenas garantir que o contexto seja deletado se ainda não foi disposed
        try
        {
            _context.Database.EnsureDeleted();
        }
        catch (ObjectDisposedException)
        {
            // Context já foi disposed pelo UnitOfWork, ignorar
        }
        GC.SuppressFinalize(this);
    }

    private User CreateTestUser(string nome, string email)
    {
        return new User
        {
            Id = Interlocked.Increment(ref _userIdCounter),
            Nome = nome,
            Email = email,
            Senha = "hashedpassword123",
            Telefone = "11999999999"
        };
    }

    private Time CreateTestTime(string nome, string slug)
    {
        return new Time
        {
            Id = Interlocked.Increment(ref _timeIdCounter),
            Nome = nome,
            Slug = slug,
            TitulosTime = 0
        };
    }

    // ========== TESTES DE INICIALIZAÇÃO LAZY DOS REPOSITÓRIOS ==========

    [Fact]
    public void Users_ShouldReturnUserRepository()
    {
        // Act
        var repository = _unitOfWork.Users;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IUserRepository>(repository);
    }

    [Fact]
    public void Users_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Users;
        var second = _unitOfWork.Users;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Times_ShouldReturnTimeRepository()
    {
        // Act
        var repository = _unitOfWork.Times;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<ITimeRepository>(repository);
    }

    [Fact]
    public void Times_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Times;
        var second = _unitOfWork.Times;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Jogadores_ShouldReturnJogadorRepository()
    {
        // Act
        var repository = _unitOfWork.Jogadores;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IJogadorRepository>(repository);
    }

    [Fact]
    public void Jogadores_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Jogadores;
        var second = _unitOfWork.Jogadores;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Campeonatos_ShouldReturnCampeonatoRepository()
    {
        // Act
        var repository = _unitOfWork.Campeonatos;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<ICampeonatoRepository>(repository);
    }

    [Fact]
    public void Campeonatos_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Campeonatos;
        var second = _unitOfWork.Campeonatos;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Partidas_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.Partidas;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<Partida>>(repository);
    }

    [Fact]
    public void Partidas_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Partidas;
        var second = _unitOfWork.Partidas;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Noticias_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.Noticias;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<Noticia>>(repository);
    }

    [Fact]
    public void Noticias_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Noticias;
        var second = _unitOfWork.Noticias;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Conquistas_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.Conquistas;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<Conquista>>(repository);
    }

    [Fact]
    public void Conquistas_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Conquistas;
        var second = _unitOfWork.Conquistas;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Jogos_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.Jogos;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<Jogo>>(repository);
    }

    [Fact]
    public void Jogos_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Jogos;
        var second = _unitOfWork.Jogos;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void Produtos_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.Produtos;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<Produto>>(repository);
    }

    [Fact]
    public void Produtos_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.Produtos;
        var second = _unitOfWork.Produtos;

        // Assert
        Assert.Same(first, second);
    }

    [Fact]
    public void InscricoesCampeonato_ShouldReturnRepository()
    {
        // Act
        var repository = _unitOfWork.InscricoesCampeonato;

        // Assert
        Assert.NotNull(repository);
        Assert.IsAssignableFrom<IRepository<InscricaoCampeonato>>(repository);
    }

    [Fact]
    public void InscricoesCampeonato_ShouldReturnSameInstance_WhenAccessedMultipleTimes()
    {
        // Act
        var first = _unitOfWork.InscricoesCampeonato;
        var second = _unitOfWork.InscricoesCampeonato;

        // Assert
        Assert.Same(first, second);
    }

    // ========== TESTES DE SAVECHANGES ==========

    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChangesToDatabase()
    {
        // Arrange
        var user = CreateTestUser("Test User", "test@example.com");
        await _context.Users.AddAsync(user);

        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnZero_WhenNoChanges()
    {
        // Act
        var result = await _unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(0, result);
    }

    // ========== TESTES DE TRANSAÇÃO (com InMemory ignorando warnings) ==========

    [Fact]
    public async Task BeginTransactionAsync_ShouldNotThrow()
    {
        // Act & Assert - Não deve lançar exceção (InMemory ignora transações)
        await _unitOfWork.BeginTransactionAsync();
    }

    [Fact]
    public async Task CommitAsync_ShouldSaveChanges()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();
        var user = CreateTestUser("Transaction User", "transaction@example.com");
        await _context.Users.AddAsync(user);

        // Act
        await _unitOfWork.CommitAsync();

        // Assert
        var savedUser = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task RollbackAsync_ShouldNotThrow_WhenNoActiveTransaction()
    {
        // Act & Assert - Não deve lançar exceção
        await _unitOfWork.RollbackAsync();
    }

    [Fact]
    public async Task RollbackAsync_ShouldNotThrow_WhenCalled()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();

        // Act & Assert - Não deve lançar exceção
        await _unitOfWork.RollbackAsync();
    }

    // ========== TESTES DE EXECUTAR EM TRANSAÇÃO ==========

    [Fact]
    public async Task ExecuteInTransactionAsync_ShouldCommit_WhenActionSucceeds()
    {
        // Arrange
        var user = CreateTestUser("Execute User", "execute@example.com");

        // Act
        await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await _context.Users.AddAsync(user);
        });

        // Assert
        var savedUser = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(savedUser);
    }

    [Fact]
    public async Task ExecuteInTransactionAsync_ShouldThrow_WhenActionThrows()
    {
        // Arrange & Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                await Task.CompletedTask;
                throw new InvalidOperationException("Test exception");
            });
        });
    }

    // ========== TESTES DE DISPOSE ==========

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<FemeeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new FemeeDbContext(options);
        var unitOfWork = new UnitOfWork(context);

        // Act
        unitOfWork.Dispose();

        // Assert - Tentar usar o contexto deve falhar
        Assert.Throws<ObjectDisposedException>(() => context.Users.ToList());
    }

    // ========== TESTES DE INTEGRAÇÃO DOS REPOSITÓRIOS ==========

    [Fact]
    public async Task Users_Repository_ShouldWorkWithUnitOfWork()
    {
        // Arrange
        var user = CreateTestUser("Integration User", "integration@example.com");

        // Act
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var foundUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
        Assert.NotNull(foundUser);
        Assert.Equal("Integration User", foundUser.Nome);
    }

    [Fact]
    public async Task Times_Repository_ShouldWorkWithUnitOfWork()
    {
        // Arrange
        var time = CreateTestTime("Integration Time", "integration-time");

        // Act
        await _unitOfWork.Times.AddAsync(time);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var foundTime = await _unitOfWork.Times.GetByIdAsync(time.Id);
        Assert.NotNull(foundTime);
        Assert.Equal("Integration Time", foundTime.Nome);
    }

    [Fact]
    public async Task Partidas_Repository_ShouldWorkWithUnitOfWork()
    {
        // Arrange
        var timeA = CreateTestTime("Time A", "time-a");
        var timeB = CreateTestTime("Time B", "time-b");
        await _context.Times.AddRangeAsync(timeA, timeB);
        await _context.SaveChangesAsync();

        var partida = new Partida
        {
            Id = Interlocked.Increment(ref _partidaIdCounter),
            TimeAId = timeA.Id,
            TimeBId = timeB.Id,
            DataHora = DateTime.UtcNow,
            Local = "Arena Test"
        };

        // Act
        await _unitOfWork.Partidas.AddAsync(partida);
        await _unitOfWork.SaveChangesAsync();

        // Assert
        var foundPartida = await _unitOfWork.Partidas.GetByIdAsync(partida.Id);
        Assert.NotNull(foundPartida);
        Assert.Equal("Arena Test", foundPartida.Local);
    }

    // ========== TESTE DE MÚLTIPLOS REPOSITÓRIOS ==========

    [Fact]
    public void AllRepositories_ShouldBeAccessible()
    {
        // Act & Assert - Verifica que todos os repositórios podem ser acessados
        Assert.NotNull(_unitOfWork.Users);
        Assert.NotNull(_unitOfWork.Times);
        Assert.NotNull(_unitOfWork.Jogadores);
        Assert.NotNull(_unitOfWork.Campeonatos);
        Assert.NotNull(_unitOfWork.Partidas);
        Assert.NotNull(_unitOfWork.Noticias);
        Assert.NotNull(_unitOfWork.Conquistas);
        Assert.NotNull(_unitOfWork.Jogos);
        Assert.NotNull(_unitOfWork.Produtos);
        Assert.NotNull(_unitOfWork.InscricoesCampeonato);
    }
}
