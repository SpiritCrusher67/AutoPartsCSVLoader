namespace AutoPartsCSVLoader.Interfaces
{
    public interface IInboxMessagesService
    {
        int LastCheckedMessageId { get; }

        Task LoadDataFromAvaliableMessage(CancellationToken cancellationToken);

        Task InitializeConnection(CancellationToken cancellationToken);

        Task InitializeLastMessageId(CancellationToken cancellationToken);
    }
}