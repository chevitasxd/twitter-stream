using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace backend_stream
{
    public class TweetEventHandler: ITweetHandler
    {
        private readonly string _connectionString;
        private readonly string _twitterChannel;
        private readonly ILogger<TweetEventHandler> _logger;
        private ConnectionMultiplexer _redis;
        private ISubscriber _sub;

        public event EventHandler<string> OnTweetEventHandled;

        public TweetEventHandler(IConfiguration config, ILogger<TweetEventHandler> logger)
        {
             _connectionString = config["REDIS_CONNSTR"];
            _twitterChannel = config["REDIS_TWEET_CHANNEL"];
            _logger = logger;
        }

         public void Connect()
        {
            _redis = ConnectionMultiplexer.Connect(_connectionString);
            _logger.LogInformation("connected to redis");
            _sub = _redis.GetSubscriber();
            _sub.Subscribe(_twitterChannel).OnMessage(HandleEvent);            
        }

        private void HandleEvent(ChannelMessage channelMessage)
        {
            _logger.LogInformation($"Handled tweet '{channelMessage.Message}'");
            if(channelMessage.Message.HasValue && OnTweetEventHandled != null)
            {                
                OnTweetEventHandled(this, (string) channelMessage.Message);                
            }
        }

        public void Disconnect()
        {
            _logger.LogInformation("Stopping handler");
            _sub.UnsubscribeAll();
            _redis.CloseAsync();
            OnTweetEventHandled = null;
        }
    }
}