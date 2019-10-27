using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace twitter_poller
{
    public interface ITweetPublisher
    {
        Task RunAsync(CancellationToken stoppingToken);
        void PublishTweetEvent(string tweetContent);
    }

    public class RedisTweetPublisher : ITweetPublisher
    {
        private readonly string _connectionString;
        private readonly string _twiiterChannel;
        private readonly ILogger<RedisTweetPublisher> _logger;
        private ConnectionMultiplexer _redis;
        private ISubscriber _sub;


        public RedisTweetPublisher(IConfiguration config, ILogger<RedisTweetPublisher> logger)
        {
            _connectionString = config["REDIS_CONNSTR"];            
            _twiiterChannel = config["REDIS_TWEET_CHANNEL"];            
            _logger = logger;            
        }

        public void PublishTweetEvent(string tweetEvent)
        {
             _logger.LogDebug($"Publishing to redis {_twiiterChannel}");
            _sub.Publish(_twiiterChannel, tweetEvent);
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            _redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);
            _sub = _redis.GetSubscriber();
            stoppingToken.Register(async () => {
                _logger.LogDebug("Closing redis connection");
                await _redis.CloseAsync();
            });
        }
    }
}