using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Principal;

namespace FEMEE.Application.Interfaces.Repositories
{
    /// <summary>
    /// Interface genérica para repositórios.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Busca todas as entidades.
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Busca uma entidade pelo ID.
        /// </summary>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Busca entidades que correspondem ao predicado.
        /// A expressão é traduzida para SQL e executada no servidor.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adiciona uma nova entidade.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Remove uma entidade pelo ID.
        /// </summary>
        Task DeleteAsync(int id);
    }
}