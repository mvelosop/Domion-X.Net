using Domion.Web.Navigation;
using Domion.Web.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Domion.Web.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class SessionNavigationExtensions_Tests
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

            var routeValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index" });
            var initialRouteValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 2, ps = 3 });
            var updatedRouteValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index", p = 5, ps = 3 });

            ISession session = new TestSession();

            session.SaveRouteValues(new RouteValueDictionary(initialRouteValues));

            // Act -------------------------------

            session.SaveRouteValues(new RouteValueDictionary(updatedRouteValues));

            // Assert ----------------------------

            RouteValueDictionary result = session.GetRouteValues(routeValues);

            result.ShouldBeEquivalentTo(updatedRouteValues);
        }

        [Fact]
        public void GetRouteValues_ReturnsEmptyRoute_WhenNonExistingRoute()
        {
            // Arrange ---------------------------

            var routeValues = new RouteValueDictionary(new { controller = "Tenants", action = "Index" });

            ISession session = new TestSession();

            // Act -------------------------------

            RouteValueDictionary result = session.GetRouteValues(routeValues);

            // Assert ----------------------------

            result.Should().BeEmpty();
        }
    }
}
