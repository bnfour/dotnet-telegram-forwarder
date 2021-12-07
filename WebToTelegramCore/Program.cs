using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebToTelegramCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // hardcoded default
            int port = 8082;

            if (args.Length == 2 && args[0].Equals("--port") && int.TryParse(args[1], out int nonDefPort))
            {
                port = nonDefPort;
            }

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                // quick fix to prevent using solution folder for configuration files
                // instead of built binary folder, where these files are overridden with juicy secrets
                ContentRootPath = System.AppContext.BaseDirectory
            });

            builder.WebHost.UseKestrel(kestrelOptions =>
            {
                kestrelOptions.ListenLocalhost(port);
            });

            // carrying over legacy startup-based configuration (for now?)
            // lifetime-dependent config (use detailed exception page in dev environment, basically) was dropped (for now?)
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            app.MapControllers();

            app.Run();
        }
    }
}
