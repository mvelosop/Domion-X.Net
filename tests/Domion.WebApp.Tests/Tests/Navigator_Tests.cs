using Domion.WebApp.Navigation;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class Navigator_Tests
    {
        [Fact]
        public void Add_AddsRouteValues_WhenNoRoutes()
        {
            // Arrange ---------------------------

            var routeData = new RouteData();

            routeData.Values.Add("controller", "Tenants");
            routeData.Values.Add("action", "Index");

            var navigator = new Navigator();

            var queryValues = new RouteValueDictionary(new { p = 2, ps = 3 });

            RouteValueDictionary expected = new RouteValueDictionary(new
            {
                controller = "Tenants",
                action = "Index",
                p = 2,
                ps = 3
            });

            // Act -------------------------------

            navigator.AddRouteValues(routeData, queryValues);

            RouteValueDictionary result = navigator.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Add_UpdatesRouteValues_WhenExistingRoute()
        {
            // Arrange ---------------------------

            var routeData = new RouteData();

            routeData.Values.Add("controller", "Tenants");
            routeData.Values.Add("action", "Index");

            var navigator = new Navigator();

            var queryValues = new RouteValueDictionary(new { p = 2, ps = 3 });

            RouteValueDictionary expected = new RouteValueDictionary(new
            {
                controller = "Tenants",
                action = "Index",
                p = 3,
                ps = 3
            });

            navigator.AddRouteValues(routeData, queryValues);

            // Act -------------------------------

            navigator.AddRouteValues(routeData, new RouteValueDictionary(new { p = 3, ps = 3 }));

            RouteValueDictionary result = navigator.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
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

        //    result.Should().ShouldBeEquivalentTo(expected);

        //}

        private static UrlHelper CreateUrlHelper(ActionContext context)
        {
            return new UrlHelper(context);
        }



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
