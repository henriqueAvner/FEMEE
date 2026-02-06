using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace FEMEE.API.Configuration
{
    /// <summary>
    /// Configuração centralizada de logging com Serilog.
    /// </summary>
    public static class LoggingConfiguration
    {
        /// <summary>
        /// Configura Serilog com múltiplos sinks e enriquecimento.
        /// </summary>
        /// <returns>Logger configurado</returns>
        public static Logger ConfigureLogging()
        {
            // Criar pasta de logs se não existir
            var logsPath = Path.Combine(AppContext.BaseDirectory, "logs");
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }

            var logger = new LoggerConfiguration()
                // ===== NÍVEL MÍNIMO DE LOG =====
                .MinimumLevel.Information()

                // ===== SOBRESCREVER NÍVEIS PARA NAMESPACES ESPECÍFICOS =====
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)

                // ===== SINKS =====
                // Escrever no console
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")

                // Escrever em arquivo com rolling diário
                .WriteTo.File(
                    path: Path.Combine(logsPath, "femee-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    retainedFileCountLimit: 30,  // Manter últimos 30 dias
                    fileSizeLimitBytes: 104857600)  // 100 MB por arquivo

                // Escrever erros em arquivo separado
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(le => le.Level >= LogEventLevel.Error)
                    .WriteTo.File(
                        path: Path.Combine(logsPath, "errors-.txt"),
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        retainedFileCountLimit: 60))

                // ===== ENRIQUECIMENTO =====
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "FEMEE.API")
                .Enrich.WithProperty("Environment", GetEnvironment())
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()

                .CreateLogger();

            return logger;
        }

        /// <summary>
        /// Obtém o ambiente atual (Development, Staging, Production).
        /// </summary>
        private static string GetEnvironment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        }
    }
}
