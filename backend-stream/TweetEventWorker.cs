using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend_stream
{
    public class TweetEventWorker : IHostedService
    {   
        private readonly ITweetHandler _tweetHandler;
        private readonly IHubContext<TweetHub> _tweetHub;
        private readonly ILogger<TweetEventWorker> _logger;

        public TweetEventWorker(ITweetHandler tweetHandler, IHubContext<TweetHub> tweetHub ,ILogger<TweetEventWorker> logger)
        {
            _tweetHandler = tweetHandler;
            _tweetHub = tweetHub;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _tweetHandler.OnTweetEventHandled += HandleTweetEvent;
            _tweetHandler.Connect();
            
            return Task.CompletedTask;
        }

        private void HandleTweetEvent(object sender, string e)
        {
            _logger.LogInformation("Dispatching tweet event");
            _tweetHub.Clients.All.SendAsync("ReceiveTweet", e).RunSynchronously();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _tweetHandler.Disconnect();
            return Task.CompletedTask;
        }       
    }
}