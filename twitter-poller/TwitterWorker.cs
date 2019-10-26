using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace twitter_poller
{
    public class TwitterWorker : BackgroundService, ITweeterConsumer    
    {

        private readonly ILogger<TwitterWorker> _logger;
        private readonly InMemoryCredentialStore _credentials;
        private TwitterContext _twitterContext;

        public event EventHandler<string> OnTweetReceived;

        public TwitterWorker(IConfiguration config, ILogger<TwitterWorker> logger)
        {
            _logger = logger;
            _credentials = new InMemoryCredentialStore{
                ConsumerKey = config["TWITTER_API_KEY"],
                ConsumerSecret = config["TWITTER_API_SECRET"]
            };
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
           var auth = new ApplicationOnlyAuthorizer { 
               CredentialStore = _credentials
           };
           await auth.AuthorizeAsync();
           _twitterContext = new TwitterContext(auth);
        }        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));


            var stream = _twitterContext.Streaming.Where(str => str.Type == StreamingType.Filter);
            await stream.StartAsync(async strm => {
                if(OnTweetReceived != null)
                     OnTweetReceived(this, strm.Content);
                if(stoppingToken.IsCancellationRequested)
                    strm.CloseStream();
            });

            _logger.LogDebug($"GracePeriod background task is finished.");
        }        
    }
}