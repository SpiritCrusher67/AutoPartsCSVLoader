using AutoPartsCSVLoader.Data.Configuration;
using AutoPartsCSVLoader.Interfaces;
using MailKit;
using MimeKit;

namespace AutoPartsCSVLoader.Services
{
    public class InboxMessagesService : IInboxMessagesService
    {
        private readonly IMailInboxService _mailInboxService;
        private readonly SuppliersEmailData _suppliersEmailData;
        private readonly IPriceItemDataDbLoader _dbDataLoader;

        public int LastCheckedMessageId { get; private set; }

        public InboxMessagesService(IMailInboxService mailInboxService,
            IPriceItemDataDbLoader dbLoader, SuppliersEmailData suppliersEmailData) =>
            (_mailInboxService, _suppliersEmailData, _dbDataLoader) =
            (mailInboxService, suppliersEmailData, dbLoader);

        public async Task InitializeConnection(CancellationToken cancellationToken)
        {
            await _mailInboxService.ConnectAsync(cancellationToken);
            await _mailInboxService.AuthenticateAsync(cancellationToken);
        }

        public async Task InitializeLastMessageId(CancellationToken cancellationToken)
        {
            var inbox = await _mailInboxService.GetInboxAsync(cancellationToken);
            LastCheckedMessageId = inbox.Count;
        }

        public async Task LoadDataFromAvaliableMessage(CancellationToken cancellationToken)
        {
            var inbox = await _mailInboxService.GetInboxAsync(cancellationToken);
            if (IsNewMessageAvailable(inbox))
            {
                await LoadDataFromNewMessage(inbox, cancellationToken);
            }
        }

        private bool IsNewMessageAvailable(IMailFolder inbox) => inbox.Count > LastCheckedMessageId;

        private async Task LoadDataFromNewMessage(IMailFolder inbox, CancellationToken cancellationToken)
        {
            var message = await GetMessageById(inbox, LastCheckedMessageId++, cancellationToken);
            var (supplierName, stream) = await GetMessageDataOrDefault(message, cancellationToken);
            await LoadDataFromMessage(supplierName, stream, cancellationToken);
        }

        private async Task<MimeMessage> GetMessageById(IMailFolder inbox, int id,
            CancellationToken cancellationToken) =>
            await inbox.GetMessageAsync(id, cancellationToken);

        private async Task<(string, Stream)> GetMessageDataOrDefault(MimeMessage message, 
            CancellationToken cancellationToken)
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

        private async Task<Stream?> GetAttachmentDataFileStreamOrDefault(MimeMessage message,
            CancellationToken cancellationToken)
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

        private async Task LoadDataFromMessage(string supplierName, 
            Stream dataFileStream, CancellationToken cancellationToken) =>
            await _dbDataLoader.LoadDataAsync(dataFileStream, supplierName, cancellationToken);
    }
}
