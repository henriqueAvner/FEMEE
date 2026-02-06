using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FEMEE.UnitTests.Infrastructure;

/// <summary>
/// Classe base para testes de repositório.
/// Fornece um DbContext InMemory para cada teste.
/// </summary>
public abstract class RepositoryTestBase : IDisposable
{
    protected FemeeDbContext Context { get; private set; }

    protected RepositoryTestBase()
    {
        var options = new DbContextOptionsBuilder<FemeeDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new FemeeDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
