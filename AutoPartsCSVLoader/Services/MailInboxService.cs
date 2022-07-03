using AutoPartsCSVLoader.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using System.Net;

namespace AutoPartsCSVLoader.Services
{
    public class MailInboxService : IMailInboxService
    {
        private readonly ImapClient _client;
        private readonly IConfiguration _configuration;

        public MailInboxService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new();

        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            var host = _configuration["MailConfiguration:Host"];
            var port = int.Parse(_configuration["MailConfiguration:Port"]);
            var useSSL = bool.Parse(_configuration["MailConfiguration:UseSSL"]);

            await _client.ConnectAsync(host, port, useSSL, cancellationToken);
        }

        public async Task AuthenticateAsync(CancellationToken cancellationToken = default)
        {
            var userName = _configuration["MailAuthenticationData:Email"];
            var password = _configuration["MailAuthenticationData:Password"];
            var credentials = new NetworkCredential(userName, password);

            await _client.AuthenticateAsync(credentials, cancellationToken);
        }
        public async Task<IMailFolder> GetInboxAsync(CancellationToken cancellationToken = default)
        {
            var inbox = _client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly, cancellationToken);

            return inbox;
        }
    }
}
