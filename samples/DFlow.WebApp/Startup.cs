using Autofac;
using cloudscribe.Web.Common;
using DFlow.Tenants.Lib.Data;
using DFlow.Tenants.Setup;
using DFlow.WebApp.Data;
using DFlow.WebApp.Features.Tenants;
using DFlow.WebApp.Models;
using DFlow.WebApp.Services;
using Domion.WebApp.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using Domion.WebApp.Alerts;

namespace DFlow.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Seq("http://localhost:5341")
                //.WriteTo.RollingFile(Path.Combine(env.ContentRootPath, "logs", "log.txt"))
                .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public TenantsDbHelper DbHelper { get; private set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILifetimeScope scope)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();

                LoadDevelopmentTestData(scope);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseIdentity();
            app.UseSession();
            app.UseMvcWithDefaultRoute();

            app.UseCloudscribeCommonStaticFiles();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigureDbLogging(scope);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //builder.RegisterModule(new AutofacModule());

            var containerSetup = new TenantsContainerSetup(DbHelper);

            builder.RegisterType<HttpContextAccessor>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AlertsManager>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
                
            // cloudscribe configuration
            builder.RegisterType<CookieTempDataProvider>()
                .As<ITempDataProvider>()
                .SingleInstance();

            builder.RegisterType<GlobalResourceManagerStringLocalizerFactory>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<GlobalResourceManagerStringLocalizer>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<CloudscribeCommonResources>()
                .SingleInstance();

            builder.RegisterType<ViewAlerts>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Register application module's services
            containerSetup.RegisterTypes(builder);

            // DFlow services
            builder.RegisterType<TenantsServices>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TenantViewModelMapper>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddFeatureFolders()
                .AddRazorOptions(options =>
                {
                    options.AddCloudscribeCommonEmbeddedViews();
                });

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // cloudScribe services
            services.AddCloudscribePagination();
            services.Configure<GlobalResourceOptions>(Configuration.GetSection("GlobalResourceOptions"));

            // I chose "GlobalResources" as the folder name where the .resx files will go, but it can be whatever you choose.
            services.AddLocalization(options => options.ResourcesPath = "GlobalResources");

            SetupDatabase();
        }

        private void ConfigureDbLogging(ILifetimeScope scope)
        {
            using (ILifetimeScope subScope = scope.BeginLifetimeScope())
            {
                var dbContext = subScope.Resolve<TenantsDbContext>();

                var serviceProvider = dbContext.GetInfrastructure<IServiceProvider>();

                var dbLoggerFactory = serviceProvider.GetService<ILoggerFactory>();

                dbLoggerFactory.AddSerilog();
            }
        }

        private void LoadDevelopmentTestData(ILifetimeScope scope)
        {
            var testData = new DevelopmentTestData(scope);

            testData.Load();
        }

        private void SetupDatabase()
        {
            SetupIdentityDatabase();

            DbHelper = new TenantsDbHelper(Configuration.GetConnectionString("DefaultConnection"));

            DbHelper.SetupDatabase();
        }

        private void SetupIdentityDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            using (var dbContext = new ApplicationDbContext(optionBuilder.Options))
            {
                dbContext.Database.Migrate();
            }
        }
    }
}
