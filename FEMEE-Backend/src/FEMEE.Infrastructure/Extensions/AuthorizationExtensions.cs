
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using FEMEE.Domain.Enums;

namespace FEMEE.Infrastructure.Extensions
{
    /// <summary>
    /// Extensões para configurar políticas de autorização.
    /// Define quais roles podem acessar cada recurso.
    /// </summary>
    public static class AuthorizationExtensions
    {
        /// <summary>
        /// Adiciona políticas de autorização customizadas.
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        /// <returns>Coleção de serviços para chaining</returns>
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                // ===== POLÍTICA: ADMINISTRADOR APENAS =====
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireClaim("TipoUsuario", TipoUsuario.Administrador.ToString()));

                // ===== POLÍTICA: ADMINISTRADOR OU CAPITAO =====
                options.AddPolicy("AdminOrCapitao", policy =>
                    policy.RequireClaim("TipoUsuario",
                        TipoUsuario.Administrador.ToString(),
                        TipoUsuario.Capitao.ToString()));

                // ===== POLÍTICA: QUALQUER USUÁRIO AUTENTICADO =====
                options.AddPolicy("UsuarioAutenticado", policy =>
                    policy.RequireAuthenticatedUser());

                // ===== POLÍTICA: ADMIN, CAPITAO OU JOGADOR =====
                options.AddPolicy("UsuarioComRole", policy =>
                    policy.RequireClaim("TipoUsuario",
                        TipoUsuario.Administrador.ToString(),
                        TipoUsuario.Capitao.ToString(),
                        TipoUsuario.Jogador.ToString()));

                // ===== POLÍTICA: CAPITAO APENAS =====
                options.AddPolicy("CapitaoOnly", policy =>
                    policy.RequireClaim("TipoUsuario", TipoUsuario.Capitao.ToString()));

                // ===== POLÍTICA: JOGADOR APENAS =====
                options.AddPolicy("JogadorOnly", policy =>
                    policy.RequireClaim("TipoUsuario", TipoUsuario.Jogador.ToString()));
            });

            return services;
        }
    }
}
