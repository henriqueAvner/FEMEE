using FEMEE.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Implementação de cache em memória.
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<MemoryCacheService> _logger;
        private readonly HashSet<string> _keys = new();
        private readonly object _lock = new();

        public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<T> GetTAsync<T>(string chave)
        {
            _logger.LogDebug("Buscando cache para chave: {Chave}", chave);
            
            if (_cache.TryGetValue(chave, out T? valor))
            {
                _logger.LogDebug("Cache hit para chave: {Chave}", chave);
                return Task.FromResult(valor!);
            }

            _logger.LogDebug("Cache miss para chave: {Chave}", chave);
            return Task.FromResult(default(T)!);
        }

        public Task SetAsync<T>(string chave, T valor, TimeSpan? expiracao = null)
        {
            _logger.LogDebug("Armazenando em cache: {Chave}", chave);
            
            var options = new MemoryCacheEntryOptions();
            
            if (expiracao.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiracao.Value;
            }
            else
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
            }

            _cache.Set(chave, valor, options);
            
            lock (_lock)
            {
                _keys.Add(chave);
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string chave)
        {
            _logger.LogDebug("Removendo cache para chave: {Chave}", chave);
            _cache.Remove(chave);
            
            lock (_lock)
            {
                _keys.Remove(chave);
            }

            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _logger.LogInformation("Limpando todo o cache");
            
            lock (_lock)
            {
                foreach (var key in _keys)
                {
                    _cache.Remove(key);
                }
                _keys.Clear();
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string chave)
        {
            var exists = _cache.TryGetValue(chave, out _);
            return Task.FromResult(exists);
        }
    }
}
