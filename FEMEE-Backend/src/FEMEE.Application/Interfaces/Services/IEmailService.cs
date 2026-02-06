using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;

namespace FEMEE.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string destinatario, string assunto, string corpo);
        Task SendConfirmationEmailAsync(User usuario, string linkConfirmacao);
        Task SendPasswordResetEmailAsync(User usuario, string linkReset);
        Task SendInscriptionApprovedEmailAsync(User usuario, Campeonato campeonato);
        Task SendMatchScheduledEmailAsync(Time time, Partida partida);
    }
}