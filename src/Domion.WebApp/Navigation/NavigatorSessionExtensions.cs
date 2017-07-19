using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Domion.WebApp.Navigation
{
    public static class NavigatorSessionExtensions
    {
        public static void SaveRouteValues(this ISession session, RouteValueDictionary routeValues = null)
        {
            string key = GetKey(routeValues);

            var values = new Dictionary<string, object>();

            foreach (var item in routeValues)
            {
                values[item.Key] = item.Value;
            }

            if (routeValues != null)
            {
                foreach (var item in routeValues)
                {
                    values[item.Key] = item.Value;
                }
            }

            if (session.TryGetValue(key, out byte[] dummy))
            {
                session.Remove(key);
            }

            session.SetString(key, JsonConvert.SerializeObject(values));
        }

        public static RouteValueDictionary GetRouteValues(this ISession session, RouteValueDictionary routeValues)
        {
            string key = GetKey(routeValues);

            var value = session.GetString(key);

            if (value == null)
            {
                return new RouteValueDictionary();
            }
            {
                var routeDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

                return new RouteValueDictionary(routeDict);
            }
        }

        private static string GetKey(RouteValueDictionary routeValues)
        {
            var sb = new StringBuilder();

            sb.Append("__Domion.WebApp.Navigation.RouteNavigator:");

            if (routeValues.TryGetValue("area", out object area))
            {
                sb.Append(area);
                sb.Append("/");
            }

            if (routeValues.TryGetValue("controller", out object controller))
            {
                sb.Append(controller);
                sb.Append("/");
            }

            if (routeValues.TryGetValue("action", out object action))
            {
                sb.Append(action);
            }

            return sb.ToString().ToLower();
        }
    }
}
