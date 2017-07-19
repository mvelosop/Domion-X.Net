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
        public void SaveRouteValues_AddsRouteAndValues()
        {
            // Arrange ---------------------------

            RouteData routeData = CreateRouteData("Index", "Tenants");

            var paramsObject = new { p = 2, ps = 3 };

            ISession session = new TestSession();

            var routeObject = new { controller = "Tenants", action = "Index", p = 2, ps = 3 };

            RouteValueDictionary expected = new RouteValueDictionary(routeObject);

            // Act -------------------------------

            session.SaveRouteValues(routeData, new RouteValueDictionary(paramsObject));

            RouteValueDictionary result = session.GetRouteValues(routeData);

            // Assert ----------------------------

            result.ShouldBeEquivalentTo(expected);
        }

        private RouteData CreateRouteData(string action, string controller, string area = null)
        {
            return NavigationTestHelper.CreateRouteData(action, controller, area);
        }
    }
}
