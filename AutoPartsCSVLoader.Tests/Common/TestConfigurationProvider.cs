using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AutoPartsCSVLoader.Tests.Common
{
    public static class TestConfigurationProvider
    {
        public static IConfiguration Configuration { get; private set; }

        static TestConfigurationProvider()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"SuppliersColumnsConfiguration:Supplier1:Vendor", "Бренд"},
                {"SuppliersColumnsConfiguration:Supplier1:Number", "Каталожный номер"},
                {"SuppliersColumnsConfiguration:Supplier1:Description", "Описание"},
                {"SuppliersColumnsConfiguration:Supplier1:Price", "Цена, руб."},
                {"SuppliersColumnsConfiguration:Supplier1:Count", "Наличие"},

                {"SuppliersColumnsConfiguration:Supplier2:Vendor", "Производитель"},
                {"SuppliersColumnsConfiguration:Supplier2:Number", "Номер в каталоге"},
                {"SuppliersColumnsConfiguration:Supplier2:Description", "Описание запчасти"},
                {"SuppliersColumnsConfiguration:Supplier2:Price", "Стоимость запчасти"},
                {"SuppliersColumnsConfiguration:Supplier2:Count", "Количество"}
            };

            Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
    }
}
