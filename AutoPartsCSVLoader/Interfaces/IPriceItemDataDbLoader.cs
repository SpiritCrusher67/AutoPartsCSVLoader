namespace AutoPartsCSVLoader.Interfaces
{
    public interface IPriceItemDataDbLoader
    {
        Task LoadDataAsync(Stream dataStream, string suplierConfigurationName, CancellationToken cancellationToken = default);
    }
}
