namespace FEMEE.API.Configuration
{
    /// <summary>
    /// Configuração do Swagger com suporte a autenticação JWT.
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Adiciona configuração do Swagger.
        /// A configuração JWT Bearer requer Microsoft.OpenApi 1.x.
        /// Com Microsoft.OpenApi 2.x (usado pelo ASP.NET 10), use Swagger padrão.
        /// </summary>
        public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            return services;
        }
    }
}
