using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Domion.WebApp.Navigation
{
    public static class NavigatorSessionExtensions
    {
        public static void SaveRouteValues(this ISession session, RouteData routeData, RouteValueDictionary routeValues = null)
        {
            string key = GetKey(routeData);

            var values = new Dictionary<string, object>();

            foreach (var item in routeData.Values)
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

        public static RouteValueDictionary GetRouteValues(this ISession session, RouteData routeData)
        {
            string key = GetKey(routeData);

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

        private static string GetKey(RouteData routeData)
        {
            var sb = new StringBuilder();

            sb.Append("__Domion.WebApp.Navigation.RouteNavigator:");

            if (routeData.Values.TryGetValue("area", out object area))
            {
                sb.Append(area);
                sb.Append("/");
            }

            if (routeData.Values.TryGetValue("controller", out object controller))
            {
                sb.Append(controller);
                sb.Append("/");
            }

            if (routeData.Values.TryGetValue("action", out object action))
            {
                sb.Append(action);
            }

            return sb.ToString().ToLower();
        }
    }
}
