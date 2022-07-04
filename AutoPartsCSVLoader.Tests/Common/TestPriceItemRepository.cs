using AutoPartsCSVLoader.Data;
using AutoPartsCSVLoader.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoPartsCSVLoader.Tests.Common
{
    public class TestPriceItemRepository : IPriceItemRepository
    {
        public IList<PriceItem> PriceItems { get; private set; } = new List<PriceItem>();
        public Task AddPriceItemAsync(PriceItem priceItem)
        {
            PriceItems.Add(priceItem);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }

}
