using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace twitter_poller
{
    public class LinqToTwitterPoller : ITwitterPoller
    {
        private readonly ILogger<LinqToTwitterPoller> _logger;
        private readonly InMemoryCredentialStore _credentials;

        public LinqToTwitterPoller(IConfiguration config, ILogger<LinqToTwitterPoller> logger)
        {
            _logger = logger;
            _credentials = new InMemoryCredentialStore
            {
                ConsumerKey = config["TWITTER_API_KEY"],
                ConsumerSecret = config["TWITTER_API_SECRET"]
            };
        }

        public async Task Poll(CancellationToken stoppingToken, Action<string> tweetReceivedCallback)
        {

             stoppingToken.Register(() =>
                _logger.LogDebug($" LinqToTwitterPoller poll task is stopping."));

            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = _credentials
            };

            await auth.AuthorizeAsync();
            var twitterContext = new TwitterContext(auth);

            _logger.LogDebug($"LinqToTwitterPoller is starting to poll.");           

            var stream = twitterContext.Streaming.Where(str => str.Type == StreamingType.Filter);

            await stream.StartAsync(async strm =>
            {
                tweetReceivedCallback(strm.Content);
                if (stoppingToken.IsCancellationRequested)
                    strm.CloseStream();
            });
        }
    }
}