using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace twitter_poller
{
    public class RedisTweetPublisher : IHostedService
    {
        private readonly string _connectionString;
        private ConnectionMultiplexer _redis;
        private ISubscriber _sub;        

        public RedisTweetPublisher(IConfiguration config)
        {
             _connectionString = config["REDIS_CONNSTR"];
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);
            _sub = _redis.GetSubscriber();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _redis.CloseAsync();
        }

        public void PublishEvent(string tweetEvent)
        {
            _sub.Publish("tweets", tweetEvent );
        }
    }
}