using Microsoft.EntityFrameworkCore;

namespace AutoPartsCSVLoader.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PriceItem> PriceItems => Set<PriceItem>();

        public AppDbContext() => Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriceItem>().Property(f => f.Vendor).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.Number).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.SearchVendor).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.SearchNumber).HasMaxLength(64);
            modelBuilder.Entity<PriceItem>().Property(f => f.Description).HasMaxLength(512);
            modelBuilder.Entity<PriceItem>().Property(f => f.Cost).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
