using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;

namespace FEMEE.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users {get;}
        ITimeRepository Times {get;}

        IJogadorRepository Jogadores {get;}
        ICampeonatoRepository Campeonatos {get;}

        IRepository<Partida> Partidas {get;}
        IRepository<Noticia> Noticias {get;}
        IRepository<Conquista> Conquistas {get; }
        IRepository<Jogo> Jogos {get;}
        IRepository<Produto> Produtos {get;}
        IRepository<InscricaoCampeonato> InscricoesCampeonato {get;}

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task ExecuteInTransactionAsync(Func<Task> action);

    }
}
