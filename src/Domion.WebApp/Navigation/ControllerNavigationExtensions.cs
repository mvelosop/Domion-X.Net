using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Navigation
{
    public static class ControllerNavigationExtensions
    {
        public static void SaveRouteValues(this Controller controller)
        {
            ISession session = controller.HttpContext.Session;

            var routeValues = new RouteValueDictionary();

            foreach (var item in controller.ControllerContext.RouteData.Values)
            {
                routeValues.Add(item.Key, item.Value);
            }

            foreach (var item in controller.ControllerContext.HttpContext.Request.Query)
            {
                routeValues.Add(item.Key, item.Value[0]);
            }

            session.SaveRouteValues(routeValues);
        }
    }
}
