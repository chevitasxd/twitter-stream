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
                var useDummyPollerSetting = hostContext.Configuration["POLLER_USE_DUMMY"];
                bool.TryParse(useDummyPollerSetting, out bool useDummyPoller);                
                if(useDummyPoller)
                    services.AddSingleton<ITwitterPoller, DummyTwitterPoller>();
                else services.AddSingleton<ITwitterPoller, LinqToTwitterPoller>();

                services.AddSingleton<ITweetPublisher, RedisTweetPublisher>();
                services.AddSingleton<RedisTweetPublisher>();
                services.AddHostedService<TwitterWorker>();                
            });
            builder.Build().Run();
        }
    }
}
