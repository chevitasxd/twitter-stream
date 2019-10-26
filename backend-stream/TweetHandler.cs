using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace backend_stream
{
    public class TweetHandler : IHostedService, ITweetHandler
    {
        private readonly string _connectionString;
        private readonly string _twitterChannel;        
        private ConnectionMultiplexer _redis;
        private ISubscriber _sub;

        public event EventHandler<string> OnTweetEventHandled;

        public TweetHandler(IConfiguration config)
        {
            _connectionString = config["REDIS_CONNSTR"];
            _twitterChannel = config["REDIS_TWITTER_CHANNEL"];
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);
            _sub = _redis.GetSubscriber();
            _sub.Subscribe(_twitterChannel).OnMessage(HandleEvent);
        }

        private void HandleEvent(ChannelMessage channelMessage)
        {
            if(channelMessage.Message.HasValue && OnTweetEventHandled != null)
            {
                OnTweetEventHandled(this, (string) channelMessage.Message);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _sub.UnsubscribeAllAsync();
            await _redis.CloseAsync();
            OnTweetEventHandled = null;
        }
    }
}