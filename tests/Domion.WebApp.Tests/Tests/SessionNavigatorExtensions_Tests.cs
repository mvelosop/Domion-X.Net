using Domion.WebApp.Navigation;
using Domion.WebApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class SessionNavigatorExtensions_Tests
    {
        [Fact]
        public void SaveRouteValues_AddsRoute_WhenNoParams()
        {
            // Arrange ---------------------------

            var routeValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index" });

            ISession session = new TestSession();

            // Act -------------------------------

            session.SaveRouteValues(routeValues);

            // Assert ----------------------------

            RouteValueDictionary result = session.GetRouteValues(routeValues);

            result.ShouldBeEquivalentTo(routeValues);
        }

        [Fact]
        public void SaveRouteValues_AddsRouteAndParams_WhenNewRoute()
        {
            // Arrange ---------------------------

            var routeValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index" });
            var paramsValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 2, ps = 3 });

            ISession session = new TestSession();

            // Act -------------------------------

            session.SaveRouteValues(paramsValues);

            // Assert ----------------------------

            RouteValueDictionary result = session.GetRouteValues(routeValues);

            result.ShouldBeEquivalentTo(paramsValues);
        }

        [Fact]
        public void SaveRouteValues_UpdatesParams_WhenExistingRoute()
        {
            // Arrange ---------------------------

            var routeObject = new RouteValueDictionary(new { controller = "Tenants", action = "Index" });
            var initialRouteObject = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 2, ps = 3 });
            var updatedRouteObject = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 5, ps = 3 });

            ISession session = new TestSession();

            session.SaveRouteValues(new RouteValueDictionary(initialRouteObject));

            // Act -------------------------------

            session.SaveRouteValues(new RouteValueDictionary(updatedRouteObject));

            // Assert ----------------------------

            RouteValueDictionary result = session.GetRouteValues(new RouteValueDictionary(routeObject));

            result.ShouldBeEquivalentTo(updatedRouteObject);
        }
    }
}
