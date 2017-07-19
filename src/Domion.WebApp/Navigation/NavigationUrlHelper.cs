using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace Domion.WebApp.Navigation
{
    public class NavigationUrlHelper
    {
        private readonly ActionContext ActionContext;
        private readonly RouteValueDictionary RouteValueDictionary;
        private readonly RouteValueDictionary AmbientValues;

        public NavigationUrlHelper(ActionContext actionContext)
        {
            ActionContext = actionContext ?? throw new ArgumentNullException(nameof(actionContext));

            AmbientValues = ActionContext.RouteData.Values;
            RouteValueDictionary = new RouteValueDictionary();
        }

        // Adapted from UrlHelper at aspnet/Mvc
        public RouteValueDictionary ReturnRoute(UrlActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var valuesDictionary = GetValuesDictionary(actionContext.Values);

            if (actionContext.Action == null)
            {
                object action;
                if (!valuesDictionary.ContainsKey("action") &&
                    AmbientValues.TryGetValue("action", out action))
                {
                    valuesDictionary["action"] = action;
                }
            }
            else
            {
                valuesDictionary["action"] = actionContext.Action;
            }

            if (actionContext.Controller == null)
            {
                object controller;
                if (!valuesDictionary.ContainsKey("controller") &&
                    AmbientValues.TryGetValue("controller", out controller))
                {
                    valuesDictionary["controller"] = controller;
                }
            }
            else
            {
                valuesDictionary["controller"] = actionContext.Controller;
            }

            RouteValueDictionary returnRouteValues = ActionContext.HttpContext.Session.GetRouteValues(valuesDictionary);

            return returnRouteValues;
        }

        // Taken from UrlHelper at aspnet/Mvc
        private RouteValueDictionary GetValuesDictionary(object values)
        {
            // Perf: RouteValueDictionary can be cast to IDictionary<string, object>, but it is
            // special cased to avoid allocating boxed Enumerator.
            var routeValuesDictionary = values as RouteValueDictionary;
            if (routeValuesDictionary != null)
            {
                RouteValueDictionary.Clear();
                foreach (var kvp in routeValuesDictionary)
                {
                    RouteValueDictionary.Add(kvp.Key, kvp.Value);
                }

                return RouteValueDictionary;
            }

            var dictionaryValues = values as IDictionary<string, object>;
            if (dictionaryValues != null)
            {
                RouteValueDictionary.Clear();
                foreach (var kvp in dictionaryValues)
                {
                    RouteValueDictionary.Add(kvp.Key, kvp.Value);
                }

                return RouteValueDictionary;
            }

            return new RouteValueDictionary(values);
        }
    }
}
