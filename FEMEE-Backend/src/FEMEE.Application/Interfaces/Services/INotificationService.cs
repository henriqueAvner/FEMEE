using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FEMEE.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, string titulo, 
        string mensagem, string tipo = "info");

        Task SendTeamNotificationAsync(int timeId, string titulo, string mensagem);

        Task <IEnumerable<string>> GetUnreadNotificationsAsync(int userId);

        Task MarkAsReadAsync(int notificacaoId);
    }
}