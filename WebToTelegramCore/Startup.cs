using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // singleton makes changes to non-db properties persistent
            services.AddDbContext<RecordContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Singleton, ServiceLifetime.Singleton);

            services.AddSingleton<ITokenGeneratorService, TokenGeneratorService>();

            services.AddSingleton<ITelegramBotService, TelegramBotService>();

            services.AddScoped<IOwnApiService, OwnApiService>();

            // Options pattern to the rescue?
            services.Configure<CommonOptions>(Configuration.GetSection("General"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
