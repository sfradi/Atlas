
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Container = SimpleInjector.Container;

namespace AtlasQuantumAPI
{
    public class Program
    {
        private static Container container;
        private static Serilog.ILogger logger;

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

           }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
