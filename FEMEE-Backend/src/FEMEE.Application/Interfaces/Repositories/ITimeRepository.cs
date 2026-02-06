using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;

namespace FEMEE.Application.Interfaces.Repositories
{
    public interface ITimeRepository : IRepository<Time>
    {
        Task<Time> GetBySlugAsync(string slug);
        Task<IEnumerable<Time>> GetRankingAsync(int top = 10);
        Task<IEnumerable<Time>> GetTimesWithPlayersAsync();

        Task<IEnumerable<Time>> GetTimesByCampeonatoIdAsync(int campeonatoId);

        Task AtualizarVitoriasAsync(int timeId, int pontos = 3);
        Task AtualizarEmpatesAsync(int timeId, int pontos = 1);
        Task AtualizarDerrotasAsync(int timeId, int pontos = -3);

        
    }
}
