using Microsoft.EntityFrameworkCore;

namespace AutoPartsCSVLoader.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PriceItem> PriceItems => Set<PriceItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceItem>().Property(f => f.Vendor).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.Number).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.SearchVendor).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.SearchNumber).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.Description).HasMaxLength(512);
            modelBuilder.Entity<PriceItem>().Property(f => f.Price).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
