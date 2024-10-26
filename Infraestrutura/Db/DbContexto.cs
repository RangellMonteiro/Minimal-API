using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using Microsoft.Extensions.Configuration;

namespace MinimalApi.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings;

        public DbContexto(DbContextOptions<DbContexto> options, IConfiguration configuracaoAppSettings)
            : base(options)
        {
            _configuracaoAppSettings = configuracaoAppSettings;
        }

        public DbSet<Administrador> Administradores { get; set; } = default!;

        // O método OnConfiguring não é necessário se você estiver passando as opções pelo construtor.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Normalmente, você não deve usar OnConfiguring se já está configurando o DbContexto na Startup.
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("ConexaoPadrao");
                
                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseSqlServer(stringConexao);
                }
                else
                {
                    throw new InvalidOperationException("A string de conexão não foi configurada.");
                }
            }
        }
    }
}
