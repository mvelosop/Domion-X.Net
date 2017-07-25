﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using SereneDemo.AppServices;
using Serenity;
using Serenity.Abstractions;
using Serenity.Data;
using Serenity.Extensions.DependencyInjection;
using Serenity.Localization;
using Serenity.Services;
using Serenity.Web.Middleware;
using System;
using System.Data.SqlClient;
using System.IO;
using Autofac;
using Serilog;
using DFlow.Tenants.Setup;

using Log = Serilog.Log;

namespace SereneDemo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.machine.json", optional: true)
                .AddEnvironmentVariables();

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

        public static void RegisterDataProviders()
        {
            DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);
            DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", Microsoft.Data.Sqlite.SqliteFactory.Instance);

            // to enable FIREBIRD: add FirebirdSql.Data.FirebirdClient reference, set connections, and uncomment line below
            // DbProviderFactories.RegisterFactory("FirebirdSql.Data.FirebirdClient", FirebirdSql.Data.FirebirdClient.FirebirdClientFactory.Instance);

            // to enable MYSQL: add MySql.Data reference, set connections, and uncomment line below
            // DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySql.Data.MySqlClient.MySqlClientFactory.Instance);

            // to enable POSTGRES: add Npgsql reference, set connections, and uncomment line below
            // DbProviderFactories.RegisterFactory("Npgsql", Npgsql.NpgsqlFactory.Instance);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, ILifetimeScope scope)
        {
            Serenity.Extensibility.ExtensibilityHelper.SelfAssemblies = new System.Reflection.Assembly[]
            {
                typeof(LocalTextRegistry).GetAssembly(),
                typeof(SqlConnections).GetAssembly(),
                typeof(Row).GetAssembly(),
                typeof(SaveRequestHandler<>).GetAssembly(),
                typeof(WebSecurityHelper).GetAssembly(),
                typeof(Startup).GetAssembly()
            };

            SqlSettings.AutoQuotedIdentifiers = true;
            RegisterDataProviders();

            Dependency.SetResolver(new AppServices.DependencyResolver(app.ApplicationServices));

            var textRegistry = app.ApplicationServices.GetRequiredService<ILocalTextRegistry>();
            textRegistry.AddNestedTexts();
            textRegistry.AddNestedPermissions();
            textRegistry.AddEnumTexts();
            textRegistry.AddRowTexts();
            var contentRoot = env.ContentRootPath;
            textRegistry.AddJsonTexts(System.IO.Path.Combine(env.WebRootPath, "Scripts/serenity/texts".Replace('/', Path.DirectorySeparatorChar)));
            textRegistry.AddJsonTexts(System.IO.Path.Combine(env.WebRootPath, "Scripts/site/texts".Replace('/', Path.DirectorySeparatorChar)));
            textRegistry.AddJsonTexts(System.IO.Path.Combine(env.ContentRootPath, "App_Data/texts".Replace('/', Path.DirectorySeparatorChar)));

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var reqLocOpt = new RequestLocalizationOptions();
            reqLocOpt.SupportedUICultures = UserCultureProvider.SupportedCultures;
            reqLocOpt.SupportedCultures = UserCultureProvider.SupportedCultures;
            reqLocOpt.RequestCultureProviders.Clear();
            reqLocOpt.RequestCultureProviders.Add(new UserCultureProvider());
            app.UseRequestLocalization(reqLocOpt);

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
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "CookieAuthenticationScheme",
                CookieName = ".AspNetAuth",
                LoginPath = new PathString("/Account/Login/"),
                AccessDeniedPath = new PathString("/Account/AccessDenied"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });

            app.UseDynamicScripts();
            app.UseMvc(routes =>
            {
            });

            DataMigrations.Initialize();
            
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
            var containerSetup = new TenantsContainerSetup(DbHelper);

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
            services.AddMvc(options =>
            {
                options.ModelBinderProviders.Insert(0, new ServiceEndpointModelBinderProvider());
                options.Conventions.Add(new ServiceEndpointActionModelConvention());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddConfig(Configuration);
            services.AddCaching();
            services.AddTextRegistry();
            services.AddFileLogging();
            services.AddSingleton<IAuthenticationService, Administration.AuthenticationService>();
            services.AddSingleton<IAuthorizationService, Administration.AuthorizationService>();
            services.AddSingleton<IUserRetrieveService, Administration.UserRetrieveService>();
            services.AddSingleton<IPermissionService, Administration.PermissionService>();

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