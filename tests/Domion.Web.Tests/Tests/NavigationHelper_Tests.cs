using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Domion.Web.Navigation;
using Domion.Web.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using NSubstitute;
using Xunit;

//------------------------------------------------------------------------------------------------------------------------------------------
// Support methods taken from https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.Core.Test/Routing/UrlHelperTest.cs
// Release 1.1.3
//------------------------------------------------------------------------------------------------------------------------------------------

namespace Domion.Web.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class NavigationHelper_Tests
    {
        [Fact]
        public void GetReturnRoute_ReturnsRouteParams_WhenExistingRoute()
        {
            // Arrange ---------------------------

            var services = CreateServices();
            var routeBuilder = CreateRouteBuilder(services);

            routeBuilder.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            var actionContext = new ActionContext()
            {
                HttpContext = CreateHttpContext(services)
            };

            var indexRouteValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 2, ps = 3 });
            var detailsRouteValues = new RouteValueDictionary(new { controller = "Tenants", action = "Details", p = 1 });

            // Index url
            actionContext.HttpContext.Session.SaveRouteValues(indexRouteValues);

            var routeData = new RouteData();

            actionContext.RouteData = routeData;

            // This simulates being at the "Details" action
            actionContext.RouteData.AddRouteValues(detailsRouteValues);
            actionContext.RouteData.Routers.Add(routeBuilder.Build());

            var navHelper = new NavigationHelper(actionContext);

            // Act -------------------------------

            var urlActionContext = new UrlActionContext
            {
                Action = "Index"
            };

            RouteValueDictionary urlValues = navHelper.GetReturnRoute(urlActionContext);

            // Assert ----------------------------

            urlValues.Should().BeEquivalentTo(indexRouteValues);
        }

        private HttpContext CreateHttpContext(IServiceProvider services)
        {
            var httpContext = new DefaultHttpContext()
            {
                RequestServices = services
            };

            httpContext.Features.Set<ISessionFeature>(new SessionFeature() { Session = new TestSession() });

            return httpContext;
        }


        //--------------------------------------------------------------------------------------------------------------------------
        // Taken from https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.Core.Test/Routing/UrlHelperTest.cs
        //--------------------------------------------------------------------------------------------------------------------------
        private static IRouteBuilder CreateRouteBuilder(IServiceProvider services)
        {
            //var app = new Mock<IApplicationBuilder>();

            var app = Substitute.For<IApplicationBuilder>();

            //
            //app
            //    .SetupGet(a => a.ApplicationServices)
            //    .Returns(services);

            app.ApplicationServices.Returns(services);

            //return new RouteBuilder(app.Object)
            //{
            //    DefaultHandler = new PassThroughRouter(),
            //};

            return new RouteBuilder(app)
            {
                DefaultHandler = new PassThroughRouter(),
            };
        }

        //--------------------------------------------------------------------------------------------------------------------------
        // Taken from https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.Core.Test/Routing/UrlHelperTest.cs
        //--------------------------------------------------------------------------------------------------------------------------
        private static IServiceProvider CreateServices()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddLogging();
            services.AddRouting();
            services
                .AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>()
                .AddSingleton<UrlEncoder>(UrlEncoder.Default);

            return services.BuildServiceProvider();
        }

        //--------------------------------------------------------------------------------------------------------------------------
        // Taken from https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.Core.Test/Routing/UrlHelperTest.cs
        //--------------------------------------------------------------------------------------------------------------------------
        private class PassThroughRouter : IRouter
        {
            public VirtualPathData GetVirtualPath(VirtualPathContext context)
            {
                return null;
            }

            public Task RouteAsync(RouteContext context)
            {
                context.Handler = (c) => Task.FromResult(0);

                return Task.FromResult(false);
            }
        }
    }
}
