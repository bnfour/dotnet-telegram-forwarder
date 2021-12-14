using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace WebToTelegramCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // hardcoded default, because 8080 and 8081 were already taken on my machine
            int port = 8082;

            if (args.Length == 2 && args[0].Equals("--port") && int.TryParse(args[1], out int nonDefPort))
            {
                port = nonDefPort;
            }

            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                // quick fix to prevent using solution folder for configuration files
                // instead of built binary folder, where these files are overridden with juicy secrets
                ContentRootPath = AppContext.BaseDirectory
            });
            
            // this is used to force the code to use database from the current (e.g output for debug) directory
            // instead of using the file in the project root every launch
            
            // please note that this value ends with backslash, so in the connection string,
            // file name goes straight after |DataDirectory|, no slashes of any kind
            AppDomain.CurrentDomain.SetData("DataDirectory", AppContext.BaseDirectory);

            builder.WebHost.UseKestrel(kestrelOptions =>
            {
                // this won't work without a SSL proxy over it anyway,
                // so listening to localhost only
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
