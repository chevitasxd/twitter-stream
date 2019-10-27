using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace twitter_poller
{
    public class DummyTwitterPoller : ITwitterPoller
    {
        private readonly ILogger<DummyTwitterPoller> _logger;

        public DummyTwitterPoller(ILogger<DummyTwitterPoller> logger)
        {
            _logger = logger;
        }

        public async Task Poll(CancellationToken stoppingToken, Action<string> tweetReceivedCallback)
        {
            stoppingToken.Register(() =>
               _logger.LogDebug($"DummyTwitterPoller poll task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() =>
                {
                    string dummyTweet = $"Dummy tweet generated at {DateTime.Now}";
                    _logger.LogInformation($"Sending dummy tweet '{dummyTweet}'");
                    tweetReceivedCallback(dummyTweet);
                    Thread.Sleep(new TimeSpan(0,0,5));
                });
            }
            _logger.LogDebug("Stopping dummy tweet poller");
        }
    }
}