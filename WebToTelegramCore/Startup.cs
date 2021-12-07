using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebToTelegramCore.Options;
using WebToTelegramCore.Services;

using Record = WebToTelegramCore.Models.Record;

namespace WebToTelegramCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // singleton makes changes to non-db properties persistent
            services.AddDbContext<RecordContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddSingleton<ITokenGeneratorService, TokenGeneratorService>();
            services.AddSingleton<ITelegramBotService, TelegramBotService>();
            services.AddSingleton<IFormatterService, FormatterService>();

            services.AddScoped<IOwnApiService, OwnApiService>();
            services.AddScoped<ITelegramApiService, TelegramApiService>();

            // quick crutch -- Telegram.Bot's update class relies on some Newtonsoft attributes,
            // so to deserialize it correctly, we need to use this library as well
            services.AddControllers().AddNewtonsoftJson();

            // Options pattern to the rescue?
            services.Configure<CommonOptions>(Configuration.GetSection("General"));
            services.Configure<BandwidthOptions>(Configuration.GetSection("Bandwidth"));
            services.Configure<LocalizationOptions>(Configuration.GetSection("Strings"));
            // loading this explicitly as there's no straightforward way to pass options
            // to models; I can be wrong though
            // TODO: see if there's a better way
            var preload = Configuration.GetSection("Bandwidth").GetValue<int>("InitialCount");
            Record.SetMaxValue(preload);
        }
    }
}
