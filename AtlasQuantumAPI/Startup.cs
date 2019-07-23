using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using AtlasQuantumAPI.Data;
using AtlasQuantumAPI.Data.SQLDatabase;
using AtlasQuantumAPI.Repository;
using AtlasQuantumAPI.Infrastructure;
using Serilog;

namespace AtlasQuantumAPI
{
    public class Startup
    {
        public static Container container;
        private static Serilog.ILogger logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            container = new Container();
            container.Options.DependencyInjectionBehavior = new SeriLogContextualLoggerInjectionBrehavior(container.Options);
            container.RegisterInstance(Log.Logger);
            container.RegisterInstance<IDataContext>(SQLConnectionContextFactory.Instance);
            container.Register<IAccountRepository, AccountRepository>();
            container.RegisterDecorator<IAccountRepository, CachedAccountRepository>();
            container.Register<IAccountHistoryRepository, AccountHistoryRepository>();
            container.RegisterDecorator<IAccountHistoryRepository, CachedAccountHistoryRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
