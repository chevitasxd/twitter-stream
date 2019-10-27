using System;
using System.Threading;
using System.Threading.Tasks;

namespace twitter_poller
{
    public interface ITwitterPoller
    {        
        Task Poll(CancellationToken stoppingToken, Action<string> tweetReceivedCallback);
    }
}