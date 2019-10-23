using System;
using Microsoft.Extensions.Hosting;

namespace twitter_poller
{
    interface ITweeterConsumer : IHostedService
    {
        event EventHandler<string> OnTweetReceived;
    }
}