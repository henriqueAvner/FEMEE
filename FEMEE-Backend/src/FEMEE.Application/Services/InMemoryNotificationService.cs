using FEMEE.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Implementação do serviço de notificações em memória.
    /// Em produção, pode ser integrado com SignalR, Firebase, ou outro serviço de push.
    /// </summary>
    public class InMemoryNotificationService : INotificationService
    {
        private readonly ILogger<InMemoryNotificationService> _logger;
        private readonly Dictionary<int, List<NotificationItem>> _notifications = new();
        private readonly object _lock = new();
        private int _notificationIdCounter = 1;

        public InMemoryNotificationService(ILogger<InMemoryNotificationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task SendNotificationAsync(int userId, string titulo, string mensagem, string tipo = "info")
        {
            _logger.LogInformation("Enviando notificação para usuário {UserId}: {Titulo}", userId, titulo);

            lock (_lock)
            {
                if (!_notifications.ContainsKey(userId))
                {
                    _notifications[userId] = new List<NotificationItem>();
                }

                _notifications[userId].Add(new NotificationItem
                {
                    Id = _notificationIdCounter++,
                    UserId = userId,
                    Titulo = titulo,
                    Mensagem = mensagem,
                    Tipo = tipo,
                    DataCriacao = DateTime.UtcNow,
                    Lida = false
                });
            }

            return Task.CompletedTask;
        }

        public Task SendTeamNotificationAsync(int timeId, string titulo, string mensagem)
        {
            _logger.LogInformation("Enviando notificação para time {TimeId}: {Titulo}", timeId, titulo);
            
            // Em uma implementação real, buscaria os jogadores do time e enviaria para cada um
            // Por ora, apenas loga a intenção
            _logger.LogDebug("Notificação para time {TimeId} registrada", timeId);
            
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetUnreadNotificationsAsync(int userId)
        {
            _logger.LogDebug("Buscando notificações não lidas para usuário: {UserId}", userId);

            lock (_lock)
            {
                if (!_notifications.ContainsKey(userId))
                {
                    return Task.FromResult<IEnumerable<string>>(Array.Empty<string>());
                }

                var unread = _notifications[userId]
                    .Where(n => !n.Lida)
                    .OrderByDescending(n => n.DataCriacao)
                    .Select(n => $"[{n.Tipo.ToUpper()}] {n.Titulo}: {n.Mensagem}")
                    .ToList();

                return Task.FromResult<IEnumerable<string>>(unread);
            }
        }

        public Task MarkAsReadAsync(int notificacaoId)
        {
            _logger.LogDebug("Marcando notificação como lida: {NotificacaoId}", notificacaoId);

            lock (_lock)
            {
                foreach (var userNotifications in _notifications.Values)
                {
                    var notification = userNotifications.FirstOrDefault(n => n.Id == notificacaoId);
                    if (notification != null)
                    {
                        notification.Lida = true;
                        break;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private class NotificationItem
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Titulo { get; set; } = string.Empty;
            public string Mensagem { get; set; } = string.Empty;
            public string Tipo { get; set; } = "info";
            public DateTime DataCriacao { get; set; }
            public bool Lida { get; set; }
        }
    }
}
