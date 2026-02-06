using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FEMEE.Infrastructure.Data.Context;

namespace FEMEE.API.Controllers
{
    /// <summary>
    /// Controller para verificação de saúde da API.
    /// Usado para health checks em ambientes de produção/Docker.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly FemeeDbContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(FemeeDbContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Health check básico - verifica se a API está respondendo.
        /// </summary>
        /// <returns>Status da API</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "FEMEE-API",
                Version = "1.0.0"
            });
        }

        /// <summary>
        /// Health check detalhado - verifica API e conexão com banco de dados.
        /// </summary>
        /// <returns>Status detalhado da API e serviços</returns>
        [HttpGet("detailed")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetailed()
        {
            var healthReport = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Service = "FEMEE-API",
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Checks = new List<object>()
            };

            var checks = (List<object>)healthReport.Checks;

            // Verificar conexão com banco de dados
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                checks.Add(new
                {
                    Name = "Database",
                    Status = canConnect ? "Healthy" : "Unhealthy",
                    Description = canConnect ? "SQL Server está acessível" : "Falha na conexão com SQL Server"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no health check do banco de dados");
                checks.Add(new
                {
                    Name = "Database",
                    Status = "Unhealthy",
                    Description = $"Erro: {ex.Message}"
                });
            }

            // Verificar se há algum check com falha
            var hasUnhealthy = checks.Any(c => ((dynamic)c).Status == "Unhealthy");

            if (hasUnhealthy)
            {
                return StatusCode(503, new
                {
                    Status = "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Service = "FEMEE-API",
                    Version = "1.0.0",
                    Environment = healthReport.Environment,
                    Checks = checks
                });
            }

            return Ok(healthReport);
        }

        /// <summary>
        /// Endpoint de liveness - verifica se a aplicação está viva.
        /// </summary>
        [HttpGet("live")]
        [AllowAnonymous]
        public IActionResult Live()
        {
            return Ok(new { Status = "Alive", Timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Endpoint de readiness - verifica se a aplicação está pronta para receber tráfego.
        /// </summary>
        [HttpGet("ready")]
        [AllowAnonymous]
        public async Task<IActionResult> Ready()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    return Ok(new { Status = "Ready", Timestamp = DateTime.UtcNow });
                }
                return StatusCode(503, new { Status = "Not Ready", Reason = "Database not available" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha no readiness check");
                return StatusCode(503, new { Status = "Not Ready", Reason = ex.Message });
            }
        }
    }
}
