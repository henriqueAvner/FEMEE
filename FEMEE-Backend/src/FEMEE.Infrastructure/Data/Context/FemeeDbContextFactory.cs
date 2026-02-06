using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FEMEE.Infrastructure.Data.Context
{
    public class FemeeDbContextFactory : IDesignTimeDbContextFactory<FemeeDbContext>
    {
        public FemeeDbContext CreateDbContext(string[] args)
        {
            // 1. Localiza a pasta da API de forma robusta
            string projectPath = Directory.GetCurrentDirectory();
            
            // Se estivermos na raiz da solução, entra na pasta da API
            // Se estivermos na pasta Infrastructure, sobe um nível e entra na API
            string apiPath = Path.Combine(projectPath, "src", "FEMEE.API");
            if (!Directory.Exists(apiPath))
            {
                apiPath = Path.Combine(projectPath, "..", "FEMEE.API");
            }

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FemeeDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new FemeeDbContext(builder.Options);
        }
    }
}
