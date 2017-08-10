using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Domion.Web.Navigation
{
    public static class SessionNavigationExtensions
    {
        public static void SaveRouteValues(this ISession session, RouteValueDictionary routeValues)
        {
            if (routeValues == null) throw new ArgumentNullException(nameof(routeValues));

            string key = GetKey(routeValues);

            if (session.TryGetValue(key, out byte[] dummy))
            {
                session.Remove(key);
            }

            var routeValuesDict = new Dictionary<string, object>();

            foreach (var item in routeValues)
            {
                routeValuesDict.Add(item.Key, item.Value);
            }

            string jsonValue = JsonConvert.SerializeObject(routeValues);

            session.SetString(key, jsonValue);
        }

        public static RouteValueDictionary GetRouteValues(this ISession session, RouteValueDictionary routeValues)
        {
            if (routeValues == null) throw new ArgumentNullException(nameof(routeValues));

            string key = GetKey(routeValues);

            var value = session.GetString(key);

            if (value == null)
            {
                return new RouteValueDictionary();
            }

            var routeDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

            return new RouteValueDictionary(routeDict);
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
