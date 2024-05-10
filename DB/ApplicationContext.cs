using Microsoft.EntityFrameworkCore;

namespace ns_DB
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<BannedUser> BannedUsers { get; set; }
        // todo add other dbsets
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OnlineSnakeDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BannedUserConfiguration());
            // todo add other configs
        }
    }
}