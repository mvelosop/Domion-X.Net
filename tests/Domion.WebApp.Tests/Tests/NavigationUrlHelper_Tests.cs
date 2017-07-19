using Domion.WebApp.Navigation;
using Domion.WebApp.Tests.Helpers;
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
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

//------------------------------------------------------------------------------------------------------------------------------------------
// Support methods taken from https://github.com/aspnet/Mvc/blob/rel/1.1.3/test/Microsoft.AspNetCore.Mvc.Core.Test/Routing/UrlHelperTest.cs
// Release 1.1.3
//------------------------------------------------------------------------------------------------------------------------------------------

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class NavigationUrlHelper_Tests
    {
        [Fact]
        public void ReturnRoute__ReturnsRouteParams_WhenExistingRoute()
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

            var navUrlHelper = new NavigationUrlHelper(actionContext);

            // Act -------------------------------

            var urlActionContext = new UrlActionContext
            {
                Action = "Index"
            };

            RouteValueDictionary urlValues = navUrlHelper.ReturnRoute(urlActionContext);

            // Assert ----------------------------

            urlValues.ShouldBeEquivalentTo(indexRouteValues);
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

        private RouteData CreateRouteData(string action, string controller, string area = null)
        {
            return NavigationTestHelper.CreateRouteData(action, controller, area);
        }

        //public void Add_AddsRouteValues_WhenEmptyDictionary()
        //{
        //    // Arrange ---------------------------

        //    var services = CreateServices();
        //    var routeBuilder = CreateRouteBuilder(services);

        //    routeBuilder.MapRoute(
        //        name: "default",
        //        template: "{controller=Home}/{action=Index}/{id?}");

        //    var actionContext = new ActionContext()
        //    {
        //        HttpContext = new DefaultHttpContext()
        //        {
        //            RequestServices = services,
        //        },
        //    };

        //    var routeData = new RouteData();

        //    routeData.Values.Add("controller", "Tenants");
        //    routeData.Values.Add("action", "Index");

        //    //actionContext.RouteData.Routers.Add(routeBuilder.Build());

        //    //var urlHelper = CreateUrlHelper(actionContext);

        //    // Act -------------------------------

        //    var index = new IndexRouteValues();

        //    var queryValues = new RouteValueDictionary( new { p = 2, ps = 3 });

        //    index.Add(routeData, queryValues);

        //    RouteValueDictionary result = index.GetRouteValues(routeData);

        //    // Assert ----------------------------

        //    var expected = new RouteValueDictionary(new { controller = "Tenants", acttion = "Index", p = 2, ps = 3 });
        //}

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
