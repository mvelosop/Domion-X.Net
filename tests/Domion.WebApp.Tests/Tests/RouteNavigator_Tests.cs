using Domion.WebApp.Navigation;
using Domion.WebApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class RouteNavigator_Tests
    {
        [Fact]
        public void Add_UpdatesRouteValues_WhenExistingRoute()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var navigator = new RouteNavigator();

            RouteValueDictionary expected = new RouteValueDictionary(new
            {
                controller = "Tenants",
                action = "Index",
                p = 3,
                ps = 3
            });

            navigator.AddRouteValues(routeData, new RouteValueDictionary(new { p = 2, ps = 3 }));

            // Act -------------------------------

            navigator.AddRouteValues(routeData, new RouteValueDictionary(new { p = 3, ps = 3 }));

            RouteValueDictionary result = navigator.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void AddRouteValues_AddsBaseRoute_WhenNoQueryValues()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var navigator = new RouteNavigator();

            RouteValueDictionary expected = new RouteValueDictionary(new
            {
                controller = "Tenants",
                action = "Index"
            });

            // Act -------------------------------

            navigator.AddRouteValues(routeData);

            RouteValueDictionary result = navigator.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void AddRouteValues_AddsRouteValues_WhenNoRoutes()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var navigator = new RouteNavigator();

            RouteValueDictionary expected = new RouteValueDictionary(new
            {
                controller = "Tenants",
                action = "Index",
                p = 2,
                ps = 3
            });

            // Act -------------------------------

            navigator.AddRouteValues(routeData, new RouteValueDictionary(new { p = 2, ps = 3 }));

            RouteValueDictionary result = navigator.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        private RouteData CreateRouteData(string action, string controller, string area = null)
        {
            return NavigationTestHelper.CreateRouteData(action, controller, area);
        }
    }
}
