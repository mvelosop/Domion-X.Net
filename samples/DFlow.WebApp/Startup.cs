using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DFlow.WebApp.Data;
using DFlow.WebApp.Models;
using DFlow.WebApp.Services;
using Autofac;
using DFlow.Tenants.Setup;

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
        }

        public IConfigurationRoot Configuration { get; }

        public TenantsDbHelper DbHelper { get; private set; }

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
                .AddFeatureFolders();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //services.AddDbContext<DFlowWebAppContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DFlowWebAppContext")));

            services.AddCloudscribePagination();

            SetupDatabase();
        }

        void SetupDatabase()
        {
            SetupIdentityDatabase();

            DbHelper = new TenantsDbHelper(Configuration.GetConnectionString("DefaultConnection"));

            DbHelper.SetupDatabase();

        }

        void SetupIdentityDatabase()
        {
            var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            using (var dbContext = new ApplicationDbContext(optionBuilder.Options))
            {
                dbContext.Database.Migrate();
            }
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

            containerSetup.RegisterTypes(builder);

            builder.RegisterType<TenantsServices>()
                .InstancePerLifetimeScope();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILifetimeScope scope)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();

                LoadDevelopmentTestData(scope);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseMvcWithDefaultRoute();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        void LoadDevelopmentTestData(ILifetimeScope scope)
        {
            var testData = new DevelopmentTestData(scope);

            testData.Load();
        }
    }
}
