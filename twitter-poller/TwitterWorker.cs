using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace twitter_poller
{
    class TwitterWorker : BackgroundService
    {

        public TwitterWorker(ILogger<TwitterWorker> logger)
        {
            _logger = logger;
            _twitterContext = new TwitterContext()
        }

        public ILogger<TwitterWorker> _logger { get; }

        private TwitterContext _twitterContext;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));


            var stream = _twitterContext.Streaming.Where(str => str.Type == StreamingType.Filter);
            await stream.StartAsync(async strm => {
                await HandleStREAM(strm);
                if(stoppingToken.IsCancellationRequested)
                    strm.CloseStream();
            });

           

            _logger.LogDebug($"GracePeriod background task is finished.");
        }

        private Task HandleStREAM(StreamContent arg)
        {
            
        }
    }
}