using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Navigation
{
    public class Navigator : Dictionary<string, RouteValueDictionary>
    {
        private readonly Dictionary<string, RouteValueDictionary> Dictionary = new Dictionary<string, RouteValueDictionary>();

        public void Add(RouteData routeData, RouteValueDictionary routeValues)
        {
            string key = string.Join("/", routeData.Values.Select(rd => rd.Value.ToString().ToLower()));

            var value = new RouteValueDictionary();

            foreach (var item in routeData.Values)
            {
                value.Add(item.Key, item.Value);
            }

            foreach (var item in routeValues)
            {
                value.Add(item.Key, item.Value);
            }

            Dictionary.Add(key, value);
        }

        public RouteValueDictionary GetRouteValues(RouteData routeData)
        {
            string key = string.Join("/", routeData.Values.Select(rd => rd.Value.ToString().ToLower()));

            RouteValueDictionary value = null;

            Dictionary.TryGetValue(key, out value);

            return value;
        }
    }
}
