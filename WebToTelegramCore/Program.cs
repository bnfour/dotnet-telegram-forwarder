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

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // hardcoded default
            int port = 8082;

            if (args.Length == 2 && args[0].Equals("--port")
                && System.Int32.TryParse(args[1], out int nonDefPort))
            {
                port = nonDefPort;
            }

            return WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseUrls($"http://localhost:{port}")
                .UseStartup<Startup>();
        }
    }
}
