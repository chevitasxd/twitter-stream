using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace backend_stream
{
    public class TweetHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly ITweetHandler _tweetHandler;

        public TweetHub(IConfiguration config, ITweetHandler tweetHandler)
        {
            _config = config;
            _tweetHandler = tweetHandler;
            _tweetHandler.OnTweetEventHandled += DispatchTweetToClients;
        }

        private void DispatchTweetToClients(object sender, string tweetEvent)
        {
            Clients.All.SendAsync("ReceiveTweet", tweetEvent);
        }
    }
}