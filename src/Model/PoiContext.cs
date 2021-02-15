using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HappyTrip.Model
{
    public class PoiContext : DbContext
    {
        public DbSet<Poi> Poi { get; set; }
        public DbSet<PoiHistory> PoiHistory { get; set; }

        public DbSet<DayInformation> DayInformation { get; set; }

        public PoiContext() : base()
        { }

        public PoiContext(DbContextOptions<PoiContext> options) : base(options)
        {
        }

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

            modelBuilder.Entity<PoiHistory>()
                .HasOne(h => h.Poi)
                .WithMany(p => p.History);

            modelBuilder.Entity<DayInformation>()
                .HasKey(p => p.Date);
            modelBuilder.Entity<DayInformation>().Property(a => a.Date).ValueGeneratedNever();
        }
    }
}
