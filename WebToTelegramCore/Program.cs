using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebToTelegramCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls("http://localhost:8082")
                .UseStartup<Startup>();
    }
}
