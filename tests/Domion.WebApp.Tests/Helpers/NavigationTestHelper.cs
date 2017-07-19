using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Tests.Helpers
{
    public class NavigationTestHelper
    {
        public static RouteData CreateRouteData(string action, string controller, string area = null)
        {
            var routeData = new RouteData();

            if (!string.IsNullOrWhiteSpace(area))
            {
                routeData.Values.Add("area", area);
            }

            routeData.Values.Add("controller", controller);
            routeData.Values.Add("action", action);

            return routeData;
        }

    }
}
