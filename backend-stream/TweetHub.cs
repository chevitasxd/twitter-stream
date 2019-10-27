using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend_stream
{
    public class TweetHub : Hub
    {
        private readonly IConfiguration _config;
        private readonly ITweetHandler _tweetHandler;
        private readonly ILogger<TweetHub> _logger;

        public TweetHub(ILogger<TweetHub> logger)
        {            
            _logger = logger;            
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogDebug("Client Connected");
            return Task.CompletedTask;
        }
    }
}