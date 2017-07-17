using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace Domion.WebApp.Navigation
{
    public class RouteNavigator
    {
        private readonly Dictionary<string, RouteValueDictionary> Dictionary = new Dictionary<string, RouteValueDictionary>();

        public void AddRouteValues(RouteData routeData, RouteValueDictionary routeValues = null)
        {
            string key = GetKey(routeData);

            var value = new RouteValueDictionary();

            foreach (var item in routeData.Values)
            {
                value.Add(item.Key, item.Value);
            }

            if (routeValues != null)
            {
                foreach (var item in routeValues)
                {
                    value.Add(item.Key, item.Value);
                }
            }

            if (Dictionary.ContainsKey(key))
            {
                Dictionary.Remove(key);
            }

            Dictionary.Add(key, value);
        }

        public RouteValueDictionary GetRouteValues(RouteData routeData)
        {
            string key = GetKey(routeData);

            RouteValueDictionary value = null;

            Dictionary.TryGetValue(key, out value);

            return value;
        }

        private string GetKey(RouteData routeData)
        {
            return string.Join("/", routeData.Values.Select(rd => rd.Value.ToString().ToUpper()));
        }
    }
}
