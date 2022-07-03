using AutoPartsCSVLoader.Data.Configuration;
using AutoPartsCSVLoader.Interfaces;
using MimeKit;

namespace AutoPartsCSVLoader.Services
{
    public class InboxCheckingService : BackgroundService
    {
        private readonly PeriodicTimer _timer;
        private readonly MailService _mailService;
        private readonly SuppliersEmailData _suppliersEmailData;
        private readonly IPriceItemDataDbLoader _dbDataLoader;
        private int _lastCheckedMessageId = 0;

        public InboxCheckingService(MailService mailService, IConfiguration configuration, 
            SuppliersEmailData suppliersEmailData, IPriceItemDataDbLoader dbLoader)
        {
            _mailService = mailService;
            _suppliersEmailData = suppliersEmailData;
            _dbDataLoader = dbLoader;
            var chekingInterval = double.Parse(configuration["InboxCheckingIntervalInSeconds"]);
            _timer = new(TimeSpan.FromSeconds(chekingInterval));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeConnection(stoppingToken);
            await InitializeLastMessageId(stoppingToken);

            while (await _timer.WaitForNextTickAsync(stoppingToken) 
                && !stoppingToken.IsCancellationRequested)
            {
                await LoadDataFromNewMessage(stoppingToken);
            }
        }

        private async Task InitializeConnection(CancellationToken cancellationToken)
        {
            await _mailService.ConnectAsync(cancellationToken);
            await _mailService.AuthenticateAsync(cancellationToken);
        }

        private async Task InitializeLastMessageId(CancellationToken cancellationToken)
        {
            var inbox = await _mailService.GetInboxAsync(cancellationToken);
            _lastCheckedMessageId = inbox.Count;
        }

        private async Task LoadDataFromNewMessage(CancellationToken cancellationToken)
        {
            var inbox = await _mailService.GetInboxAsync(cancellationToken);

            if (inbox.Count > _lastCheckedMessageId)
            {
                var message = await GetMessageById(_lastCheckedMessageId++, cancellationToken);
                var (supplierName, stream) = await GetMessageDataOrDefault(message, cancellationToken);
                await LoadDataFromMessage(supplierName, stream, cancellationToken);
            }
        }
        private async Task<MimeMessage> GetMessageById(int id, CancellationToken cancellationToken)
        {
            var inbox = await _mailService.GetInboxAsync(cancellationToken);
            return await inbox.GetMessageAsync(id, cancellationToken);
        }
        private async Task<(string, Stream)> GetMessageDataOrDefault(MimeMessage message, CancellationToken cancellationToken)
        {
            var senderAddress = GetSenderAddressOrDefault(message);
            var supplierName = GetSupplierNameByEmailOrDefault(senderAddress);
            if (supplierName == null) return default;
            var stream = await GetAttachmentDataFileStreamOrDefault(message, cancellationToken);

            return stream != null ? (supplierName, stream) : default; 
        }

        private string? GetSenderAddressOrDefault(MimeMessage message) => 
            message.From.Mailboxes.SingleOrDefault()?.Address;

        private string? GetSupplierNameByEmailOrDefault(string? email) => email != null 
            ? _suppliersEmailData.Values.FirstOrDefault(pair => pair.Value == email).Key 
            : null;

        private async Task<Stream?> GetAttachmentDataFileStreamOrDefault(MimeMessage message, CancellationToken cancellationToken)
        {
            var attachemnt = message.Attachments.SingleOrDefault() as MimePart;
            if (attachemnt?.ContentType.MediaSubtype == "csv")
            {
                var stream = new MemoryStream();
                await attachemnt.Content.DecodeToAsync(stream, cancellationToken);
                stream.Position = 0;
                return stream;
            }
            return null;
        }

        private async Task LoadDataFromMessage(string supplierName, Stream dataFileStream, CancellationToken cancellationToken) =>
            await _dbDataLoader.LoadDataAsync(dataFileStream, supplierName, cancellationToken);
    }

}
