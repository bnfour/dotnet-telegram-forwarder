﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Options;
using WebToTelegramCore.Services;

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
            // singleton makes changes to non-db properties persistent
            services.AddDbContext<RecordContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddSingleton<ITelegramBotService, TelegramBotService>();

            services.AddScoped<IOwnApiService, OwnApiService>();
            services.AddScoped<ITelegramApiService, TelegramApiService>();
            services.AddScoped<IRecordService, RecordService>();

            services.AddTransient<ITokenGeneratorService, TokenGeneratorService>();

            // Telegram.Bot's Update class relies on some Newtonsoft attributes,
            // so to deserialize it correctly, we need to use this library as well
            services.AddControllers().AddNewtonsoftJson();

            services.Configure<CommonOptions>(Configuration.GetSection("General"));
            services.Configure<BandwidthOptions>(Configuration.GetSection("Bandwidth"));
        }
    }
}
