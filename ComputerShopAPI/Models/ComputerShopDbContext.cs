using Microsoft.EntityFrameworkCore;

namespace ComputerShopAPI.Models
{
    public class ComputerShopDBContext : DbContext
    {
        public ComputerShopDBContext(DbContextOptions options) : base(options)
        {
        }

        public ComputerShopDBContext()
        {
        }

        public DbSet<OSystem> OSystems { get; set; }
        public DbSet<Comps> Comps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string ConnectionString = "Server=localhost;Database=computer;User=root;Password=;";
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        }
    }
}
