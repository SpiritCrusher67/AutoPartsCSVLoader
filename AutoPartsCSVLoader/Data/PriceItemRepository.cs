using AutoPartsCSVLoader.Interfaces;

namespace AutoPartsCSVLoader.Data
{
    public class PriceItemRepository : IPriceItemRepository
    {
        private readonly AppDbContext _dbContext;

        public PriceItemRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPriceItemAsync(PriceItem priceItem) => await _dbContext.PriceItems.AddAsync(priceItem);

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
