using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FEMEE.Infrastructure.Data.Repositories
{
    /// <summary>
    /// Repositório especializado para usuários.
    /// Implementa IUserRepository com métodos específicos de User.
    /// </summary>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly FemeeDbContext _context;

        public UserRepository(FemeeDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém um usuário pelo email.
        /// Executa query direta no banco (não carrega todos os usuários).
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email != null && 
                    u.Email.ToLower() == email.ToLower());
        }

        /// <summary>
        /// Verifica se um email já está cadastrado.
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _context.Users
                .AnyAsync(u => u.Email != null && 
                    u.Email.ToLower() == email.ToLower());
        }
    }
}
