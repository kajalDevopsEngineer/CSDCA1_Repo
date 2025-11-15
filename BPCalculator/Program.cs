using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BPCalculator
{
    public class Program
    {
        public static void Main(string[] args)   // 👈 FIXED: added Main
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
