using Microsoft.AspNetCore.Routing;

namespace Domion.Web.Tests.Helpers
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
