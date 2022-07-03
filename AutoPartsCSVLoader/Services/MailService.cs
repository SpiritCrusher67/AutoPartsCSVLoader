using MailKit;
using MailKit.Net.Imap;
using System.Net;

namespace AutoPartsCSVLoader.Services
{
    public class MailService
    {
        private readonly ImapClient _client;
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new();

        }

        public async Task ConnectAsync()
        {
            var host = _configuration["MailConfiguration:Host"];
            var port = int.Parse(_configuration["MailConfiguration:Port"]);
            var useSSL = bool.Parse(_configuration["MailConfiguration:UseSSL"]);

            await _client.ConnectAsync(host, port, useSSL);
        }

        public async Task AuthenticateAsync()
        {
            var userName = _configuration["MailAuthenticationData:Email"];
            var password = _configuration["MailAuthenticationData:Password"];
            var credentials = new NetworkCredential(userName, password);

            await _client.AuthenticateAsync(credentials);
        }
        public async Task<IMailFolder> GetInboxAsync()
        {
            var inbox = _client.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);

            return inbox;
        }
    }
}
