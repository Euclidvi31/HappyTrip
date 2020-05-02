using Microsoft.EntityFrameworkCore;

namespace HappyTrip.Model
{
    public class PoiContext : DbContext
    {
        public DbSet<Poi> Poi { get; set; }
        public DbSet<PoiHistory> PoiHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:happytrip.database.windows.net,1433;Initial Catalog=HappyTrip;Persist Security Info=False;User ID=ysh;Password=huANX903;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PoiHistory>()
                .HasKey(p => new { p.PoiId, p.Date });
            modelBuilder.Entity<PoiHistory>()
                .HasIndex(p => p.Date);

            modelBuilder.Entity<Poi>()
                .HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}
