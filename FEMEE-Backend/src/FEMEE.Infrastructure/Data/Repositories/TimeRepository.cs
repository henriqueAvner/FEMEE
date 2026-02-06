using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FEMEE.Infrastructure.Data.Repositories
{
    public class TimeRepository : GenericRepository<Time>, ITimeRepository
    {
        private readonly FemeeDbContext _context;
        public TimeRepository(FemeeDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Time> GetBySlugAsync(string slug)
        {
            return await _context.Times.FirstOrDefaultAsync(t => t.Slug == slug);
        }

        public async Task<IEnumerable<Time>> GetRankingAsync(int top = 10)
        {
            return await _context.Times
                .OrderByDescending(t => t.Pontos)
                .Take(top)
                .ToListAsync();
        }

        public async Task<IEnumerable<Time>> GetTimesWithPlayersAsync()
        {
            return await _context.Times.Include(t => t.Jogadores).ToListAsync();
        }

        public async Task<IEnumerable<Time>> GetTimesByCampeonatoIdAsync(int campeonatoId)
        {
            return await _context.InscricoesCampeonatos
                .Where(i => i.CampeonatoId == campeonatoId)
                .Select(i => i.Time)
                .Distinct()
                .ToListAsync();

        }

        public async Task AtualizarVitoriasAsync(int timeId, int pontos = 3)
        {
            Time? time = await GetByIdAsync(timeId);
            if (time != null)
            {
                time.Vitorias++;
                time.Pontos += pontos;
                await UpdateAsync(time);
            }
        }

        public async Task AtualizarDerrotasAsync(int timeId, int pontos = -3)
        {
            var time = await GetByIdAsync(timeId);
            if (time != null)
            {
                time.Derrotas++;
                time.Pontos = Math.Max(0, time.Pontos + pontos);
                await UpdateAsync(time);
            }
        }

        public async Task AtualizarEmpatesAsync(int timeId, int pontos = 1)
        {
            var time = await GetByIdAsync(timeId);
            if (time != null)
            {
                time.Empates++;
                time.Pontos += pontos;
                await UpdateAsync(time);
            }
        }

    }
}
