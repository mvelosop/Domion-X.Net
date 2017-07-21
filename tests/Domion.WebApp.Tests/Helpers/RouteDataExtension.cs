using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Tests.Helpers
{
    public static class RouteDataExtension
    {
        public static void AddRouteValues(this RouteData routeData, RouteValueDictionary routeValues)
        {
            foreach (var item in routeValues)
            {
                routeData.Values.Add(item.Key, item.Value);
            }
        }
    }
}
