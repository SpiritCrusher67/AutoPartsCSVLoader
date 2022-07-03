using AutoPartsCSVLoader.Data;

namespace AutoPartsCSVLoader.Interfaces
{
    public interface IPriceItemRepository : IDisposable
    {
        Task AddPriceItemAsync(PriceItem priceItem);
        Task SaveAsync();
    }
}
