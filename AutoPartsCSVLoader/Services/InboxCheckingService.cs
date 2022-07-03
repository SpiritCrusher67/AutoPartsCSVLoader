using AutoPartsCSVLoader.Interfaces;

namespace AutoPartsCSVLoader.Services
{
    public class InboxCheckingService : BackgroundService
    {
        private readonly PeriodicTimer _timer;
        IInboxMessagesService _inboxMessagesService;

        public InboxCheckingService(IInboxMessagesService inboxMessagesService, IConfiguration configuration)
        {
            var chekingInterval = double.Parse(configuration["InboxCheckingIntervalInSeconds"]);
            _timer = new(TimeSpan.FromSeconds(chekingInterval));

            _inboxMessagesService = inboxMessagesService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _inboxMessagesService.InitializeConnection(stoppingToken);
            await _inboxMessagesService.InitializeLastMessageId(stoppingToken);

            while (await _timer.WaitForNextTickAsync(stoppingToken) 
                && !stoppingToken.IsCancellationRequested)
            {
                await _inboxMessagesService.LoadDataFromAvaliableMessage(stoppingToken);
            }
        }
    }

}
