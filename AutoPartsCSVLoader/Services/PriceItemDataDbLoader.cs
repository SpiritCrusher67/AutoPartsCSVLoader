using AutoPartsCSVLoader.Data;
using AutoPartsCSVLoader.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace AutoPartsCSVLoader.Services
{
    public class PriceItemDataDbLoader : IPriceItemDataDbLoader
    {
        private readonly ILogger<PriceItemDataDbLoader> _logger;
        private readonly IPriceItemRepository _repository;
        private readonly IConfiguration _configuration;

        public PriceItemDataDbLoader(ILogger<PriceItemDataDbLoader> logger, 
            IPriceItemRepository repository, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task LoadDataAsync(Stream dataStream, string suplierConfigurationName, 
            CancellationToken cancellationToken = default)
        {
            using (var streamReader = new StreamReader(dataStream))
            using (var csvReader = new CsvReader(streamReader, 
                new CsvConfiguration(CultureInfo.CurrentCulture) 
                { 
                    Mode = CsvMode.Escape, 
                    BadDataFound = args => _logger.LogWarning(args.RawRecord) 
                }))
            {
                RegisterClassMap(csvReader, suplierConfigurationName);

                while (csvReader.Read())
                {
                    try
                    {
                        var item = csvReader.GetRecord<PriceItem>();
                        await _repository.AddPriceItemAsync(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                        continue;
                    }
                }

                await _repository.SaveAsync();
            }
        }

        private void RegisterClassMap(CsvReader reader, string suplierName)
        {
            var cfg = _configuration.GetSection($"SuppliersColumnsConfiguration:{suplierName}");
            var map = new PriceItemMap(cfg);
            reader.Context.RegisterClassMap(map);
        }
    }
}
