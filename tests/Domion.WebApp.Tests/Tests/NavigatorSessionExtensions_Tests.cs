using Domion.WebApp.Navigation;
using Domion.WebApp.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace Domion.WebApp.Tests.Tests
{
    [Trait("Type", "Unit")]
    public class NavigatorSessionExtensions_Tests
    {
        [Fact]
        public void SaveRouteValues_AddsRoute_WhenNoParams()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var expectedObject = new { controller = "Tenants", action = "Index" };

            ISession session = new TestSession();

            RouteValueDictionary expected = new RouteValueDictionary(expectedObject);

            // Act -------------------------------

            session.SaveRouteValues(routeData);

            RouteValueDictionary result = session.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void SaveRouteValues_AddsRouteAndParams_WhenNewRoute()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var paramsObject = new { p = 2, ps = 3 };
            var expectedObject = new { controller = "Tenants", action = "Index", p = 2, ps = 3 };

            RouteValueDictionary expected = new RouteValueDictionary(expectedObject);

            ISession session = new TestSession();

            // Act -------------------------------

            session.SaveRouteValues(routeData, new RouteValueDictionary(paramsObject));

            RouteValueDictionary result = session.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void SaveRouteValues_UpdatesParams_WhenExistingRoute()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var initialParamsObject = new { p = 2, ps = 3 };
            var updatedParamsObject = new { p = 5, ps = 3 };
            var expectedObject = new { controller = "Tenants", action = "Index", p = 5, ps = 3 };

            ISession session = new TestSession();

            RouteValueDictionary expected = new RouteValueDictionary(expectedObject);

            // Act -------------------------------

            session.SaveRouteValues(routeData, new RouteValueDictionary(initialParamsObject));
            session.SaveRouteValues(routeData, new RouteValueDictionary(updatedParamsObject));

            RouteValueDictionary result = session.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        private RouteData CreateRouteData(string action, string controller, string area = null) =>
            NavigationTestHelper.CreateRouteData(action, controller, area);
    }
}
