using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Enums;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FEMEE.Infrastructure.Data.Repositories
{
    public class CampeonatoRepository : GenericRepository<Campeonato>, ICampeonatoRepository
    {
        private readonly FemeeDbContext _context;

        public CampeonatoRepository(FemeeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Campeonato>> GetCampeonatosAtivosAsync()
        {
            return await _context.Campeonatos
                .Where(c => c.Status == StatusCampeonato.InProgress ||
                            c.Status == StatusCampeonato.Open).ToListAsync();
        }
        public async Task<IEnumerable<Campeonato>> GetCampeonatosComInscricoesAbertasAsync()
        {
            return await _context.Campeonatos.Where(c => c.Status == StatusCampeonato.Open).ToListAsync();
        }

        public async Task<IEnumerable<Campeonato>> GetCampeonatosByJogoAsync(int jogoId)
        {
            return await _context.Campeonatos.Where(c => c.JogoId == jogoId).ToListAsync();
        }


        public async Task<IEnumerable<Campeonato>> GetCampeonatosComPartidasAsync()
        {
            return await _context.Campeonatos.Include(c => c.Partidas).ToListAsync();
        }

        public async Task<IEnumerable<Campeonato>> GetCampeonatosComTimesAsync()
        {
            return await _context.Campeonatos.Include(c => c.InscricoesCampeonatos).ToListAsync();
        }

        public async Task<bool> IsTimeInscritoAsync(int campeonatoId, int timeId)
        {
            return await _context.InscricoesCampeonatos.AnyAsync(i => i.CampeonatoId == campeonatoId && i.TimeId == timeId);
        }
    }
}
