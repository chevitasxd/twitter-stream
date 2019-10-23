using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace twitter_poller
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureServices((hostContext, services)=>
            {
                services.AddSingleton<ITweeterConsumer, TwitterWorker>();
                services.AddHostedService<ITweeterConsumer>();
                services.AddHostedService<RedisTweetPublisher>();
            });
            builder.Build().Run();
        }
    }
}
