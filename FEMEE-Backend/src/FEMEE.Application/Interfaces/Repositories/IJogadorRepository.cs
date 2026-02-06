using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.Interfaces.Repositories
{
    public interface IJogadorRepository : IRepository<Jogador>
    {
        Task<IEnumerable<Jogador>> GetJogadoresByTimeIdAsync(int timeId);
        Task<IEnumerable<Jogador>> GetJogadoresAtivosAsync(int timeId);

        Task<Jogador> GetByNickNameAsync(string nickName);

        Task<IEnumerable<Jogador>> GetByFuncaoAsync(FuncaoJogador funcao);

        Task<Jogador> GetWithUserAsync(int jogadorId);
        
    }
}
