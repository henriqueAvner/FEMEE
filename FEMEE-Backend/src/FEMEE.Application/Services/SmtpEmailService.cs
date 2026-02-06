using FEMEE.Application.Interfaces.Services;
using FEMEE.Domain.Entities.Campeonatos;
using FEMEE.Domain.Entities.Principal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace FEMEE.Application.Services
{
    /// <summary>
    /// Implementação do serviço de e-mail usando SMTP.
    /// </summary>
    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;
        private readonly IConfiguration _configuration;

        public SmtpEmailService(ILogger<SmtpEmailService> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SendEmailAsync(string destinatario, string assunto, string corpo)
        {
            _logger.LogInformation("Enviando e-mail para: {Destinatario}", destinatario);

            try
            {
                var smtpHost = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
                var smtpUser = _configuration["Email:SmtpUser"];
                var smtpPassword = _configuration["Email:SmtpPassword"];
                var fromEmail = _configuration["Email:FromEmail"] ?? "noreply@femee.com.br";
                var fromName = _configuration["Email:FromName"] ?? "FEMEE";

                if (string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogWarning("Configuração de e-mail não encontrada. E-mail não enviado.");
                    return;
                }

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = assunto,
                    Body = corpo,
                    IsBodyHtml = true
                };
                message.To.Add(destinatario);

                await client.SendMailAsync(message);
                _logger.LogInformation("E-mail enviado com sucesso para: {Destinatario}", destinatario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar e-mail para: {Destinatario}", destinatario);
                throw;
            }
        }

        public async Task SendConfirmationEmailAsync(User usuario, string linkConfirmacao)
        {
            var assunto = "FEMEE - Confirme seu e-mail";
            var corpo = $@"
                <html>
                <body>
                    <h2>Olá, {usuario.Nome}!</h2>
                    <p>Obrigado por se cadastrar na FEMEE - Federação Mineira de Esportes Eletrônicos.</p>
                    <p>Para confirmar seu e-mail, clique no link abaixo:</p>
                    <p><a href='{linkConfirmacao}'>Confirmar E-mail</a></p>
                    <p>Se você não realizou este cadastro, ignore este e-mail.</p>
                    <br/>
                    <p>Atenciosamente,<br/>Equipe FEMEE</p>
                </body>
                </html>";

            await SendEmailAsync(usuario.Email ?? string.Empty, assunto, corpo);
        }

        public async Task SendPasswordResetEmailAsync(User usuario, string linkReset)
        {
            var assunto = "FEMEE - Redefinição de Senha";
            var corpo = $@"
                <html>
                <body>
                    <h2>Olá, {usuario.Nome}!</h2>
                    <p>Você solicitou a redefinição de sua senha na FEMEE.</p>
                    <p>Para criar uma nova senha, clique no link abaixo:</p>
                    <p><a href='{linkReset}'>Redefinir Senha</a></p>
                    <p>Este link expira em 24 horas.</p>
                    <p>Se você não solicitou esta redefinição, ignore este e-mail.</p>
                    <br/>
                    <p>Atenciosamente,<br/>Equipe FEMEE</p>
                </body>
                </html>";

            await SendEmailAsync(usuario.Email ?? string.Empty, assunto, corpo);
        }

        public async Task SendInscriptionApprovedEmailAsync(User usuario, Campeonato campeonato)
        {
            var assunto = $"FEMEE - Inscrição Aprovada: {campeonato.Titulo}";
            var corpo = $@"
                <html>
                <body>
                    <h2>Parabéns, {usuario.Nome}!</h2>
                    <p>Sua inscrição no campeonato <strong>{campeonato.Titulo}</strong> foi aprovada!</p>
                    <p>Data de Início: {campeonato.DataInicio:dd/MM/yyyy}</p>
                    <p>Fique atento às próximas atualizações sobre o campeonato.</p>
                    <br/>
                    <p>Boa sorte!<br/>Equipe FEMEE</p>
                </body>
                </html>";

            await SendEmailAsync(usuario.Email ?? string.Empty, assunto, corpo);
        }

        public async Task SendMatchScheduledEmailAsync(Time time, Partida partida)
        {
            _logger.LogInformation("Enviando notificação de partida agendada para o time: {TimeId}", time.Id);
            
            // Em uma implementação real, buscaria os jogadores do time e enviaria e-mails
            var corpo = $@"
                <html>
                <body>
                    <h2>Nova Partida Agendada!</h2>
                    <p>Uma partida foi agendada para o time <strong>{time.Nome}</strong>.</p>
                    <p>Data: {partida.DataHora:dd/MM/yyyy HH:mm}</p>
                    <p>Prepare-se para a competição!</p>
                    <br/>
                    <p>Equipe FEMEE</p>
                </body>
                </html>";

            // Placeholder - em produção, buscaria os e-mails dos jogadores
            _logger.LogInformation("E-mail de partida agendada preparado para time {TimeNome}: {Corpo}", time.Nome, corpo);
            await Task.CompletedTask;
        }
    }
}
