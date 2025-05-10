using Microsoft.EntityFrameworkCore;

namespace NutriFitApp.WebAdmin.Data
{
    public class WebAdminDbContext : DbContext
    {
        public WebAdminDbContext(DbContextOptions<WebAdminDbContext> options)
            : base(options)
        {
        }

        // Ejemplo: Logs de actividad del administrador
        public DbSet<AdminLog> AdminLogs { get; set; } = null!;
    }

    public class AdminLog
    {
        public int Id { get; set; }
        public string Accion { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }
}
