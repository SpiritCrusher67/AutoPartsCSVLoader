using MailKit;

namespace AutoPartsCSVLoader.Interfaces
{
    public interface IMailInboxService
    {
        Task ConnectAsync(CancellationToken cancellationToken = default);

        Task AuthenticateAsync(CancellationToken cancellationToken = default);

        Task<IMailFolder> GetInboxAsync(CancellationToken cancellationToken = default);
    }
}
