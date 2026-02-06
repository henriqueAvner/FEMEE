using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Enums;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FEMEE.Infrastructure.Data.Repositories
{
    public class JogadorRepository : GenericRepository<Jogador>, IJogadorRepository
    {
        private readonly FemeeDbContext _context;

        public JogadorRepository(FemeeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<Jogador>> GetByFuncaoAsync(FuncaoJogador funcao)
        {
            var jogadoresByFuncao = _context.Jogadores.Where(j => j.Funcao == funcao).AsEnumerable();
            return Task.FromResult(jogadoresByFuncao);

        }

        public async Task<Jogador> GetByNickNameAsync(string nickName)
        {
            var playerNickname = await _context.Jogadores.FirstOrDefaultAsync(j => j.NickName == nickName);
            ArgumentNullException.ThrowIfNull(playerNickname, nameof(playerNickname));
            return playerNickname;
        }

        public async Task<IEnumerable<Jogador>> GetJogadoresAtivosAsync(int timeId)
        {
            return await _context.Jogadores.Where(j => j.TimeId == timeId && j.Status == StatusJogador.Ativo).ToListAsync();
        }

        public async Task<IEnumerable<Jogador>> GetJogadoresByTimeIdAsync(int timeId)
        {
            return await _context.Jogadores.Where(j => j.TimeId == timeId).ToListAsync();
        }

        public async Task<Jogador> GetWithUserAsync(int jogadorId)
        {
            var jogador = await _context.Jogadores.FirstOrDefaultAsync(j => j.Id == jogadorId);

            ArgumentNullException.ThrowIfNull(jogador, nameof(jogador));
            return jogador;
        }
    }
    
}
