using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace twitter_poller
{
    public class TwitterWorker : BackgroundService
    {
        private readonly ITwitterPoller _poller;
        private readonly ITweetPublisher _tweetPublisher;
        private readonly ILogger<TwitterWorker> _logger;

        public TwitterWorker(ITwitterPoller poller, ITweetPublisher tweetPublisher ,ILogger<TwitterWorker> logger)
        {
            _poller = poller;
            _tweetPublisher = tweetPublisher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _tweetPublisher.RunAsync(stoppingToken);
            await _poller.Poll(stoppingToken, RaiseTweetReceived);
        }

        private void RaiseTweetReceived(string tweetContent)
        {
            _logger.LogInformation("OnTweetRecevied");          
            _tweetPublisher.PublishTweetEvent(tweetContent);
        }        
    }
}