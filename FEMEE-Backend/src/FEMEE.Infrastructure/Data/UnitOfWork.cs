// FEMEE.Infrastructure/Data/UnitOfWork.cs

using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using FEMEE.Domain.Entities.Store;
using FEMEE.Application.Interfaces.Repositories;
using FEMEE.Infrastructure.Data.Context;
using FEMEE.Infrastructure.Data.Repositories;

namespace FEMEE.Infrastructure.Data
{
    /// <summary>
    /// Implementação do padrão Unit of Work.
    /// Coordena múltiplos repositórios e gerencia transações.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FemeeDbContext _context;

        // Repositórios privados (lazy initialization - são nullable até serem inicializados)
        private IUserRepository? _users;
        private ITimeRepository? _times;
        private IJogadorRepository? _jogadores;
        private ICampeonatoRepository? _campeonatos;
        private IRepository<Partida>? _partidas;
        private IRepository<Noticia>? _noticias;
        private IRepository<Conquista>? _conquistas;
        private IRepository<Jogo>? _jogos;
        private IRepository<Produto>? _produtos;
        private IRepository<InscricaoCampeonato>? _inscricoesCampeonato;

        public UnitOfWork(FemeeDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ===== PROPRIEDADES DE REPOSITÓRIOS =====

        /// <summary>
        /// Repositório especializado de usuários.
        /// Usa lazy initialization: cria apenas quando acessado.
        /// </summary>
        public IUserRepository Users
            => _users ??= new UserRepository(_context);

        /// <summary>
        /// Repositório especializado de times.
        /// </summary>
        public ITimeRepository Times
            => _times ??= new TimeRepository(_context);

        /// <summary>
        /// Repositório especializado de jogadores.
        /// </summary>
        public IJogadorRepository Jogadores
            => _jogadores ??= new JogadorRepository(_context);

        /// <summary>
        /// Repositório especializado de campeonatos.
        /// </summary>
        public ICampeonatoRepository Campeonatos
            => _campeonatos ??= new CampeonatoRepository(_context);

        /// <summary>
        /// Repositório de partidas.
        /// </summary>
        public IRepository<Partida> Partidas
            => _partidas ??= new GenericRepository<Partida>(_context);

        /// <summary>
        /// Repositório de notícias.
        /// </summary>
        public IRepository<Noticia> Noticias
            => _noticias ??= new GenericRepository<Noticia>(_context);

        /// <summary>
        /// Repositório de conquistas.
        /// </summary>
        public IRepository<Conquista> Conquistas
            => _conquistas ??= new GenericRepository<Conquista>(_context);

        /// <summary>
        /// Repositório de jogos.
        /// </summary>
        public IRepository<Jogo> Jogos
            => _jogos ??= new GenericRepository<Jogo>(_context);

        /// <summary>
        /// Repositório de produtos.
        /// </summary>
        public IRepository<Produto> Produtos
            => _produtos ??= new GenericRepository<Produto>(_context);

        /// <summary>
        /// Repositório de inscrições em campeonatos.
        /// </summary>
        public IRepository<InscricaoCampeonato> InscricoesCampeonato
            => _inscricoesCampeonato ??= new GenericRepository<InscricaoCampeonato>(_context);


        // ===== MÉTODOS DE TRANSAÇÃO =====

        /// <summary>
        /// Salva todas as mudanças no banco de dados.
        /// </summary>
        /// <returns>Número de registros afetados</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Inicia uma transação.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Confirma a transação.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Desfaz a transação (rollback).
        /// </summary>
        public async Task RollbackAsync()
        {
            try
            {
                await _context.Database.RollbackTransactionAsync();
            }
            catch (InvalidOperationException)
            {
                // Nenhuma transação ativa
            }
        }

        /// <summary>
        /// Executa um bloco de código dentro de uma transação.
        /// Se houver exceção, faz rollback automaticamente.
        /// </summary>
        /// <param name="action">Ação a executar</param>
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            await BeginTransactionAsync();
            try
            {
                await action();
                await CommitAsync();
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
        }

        // ===== DISPOSE =====

        /// <summary>
        /// Libera recursos do contexto.
        /// </summary>
        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
