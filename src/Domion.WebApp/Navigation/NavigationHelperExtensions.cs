using System;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Domion.WebApp.Navigation
{
    public static class NavigationHelperExtensions
    {
        public static RouteValueDictionary GetReturnRoute(this NavigationHelper helper)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(
                action: null,
                controller: null,
                values: null,
                protocol: null,
                host: null,
                fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(this NavigationHelper helper, string action)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller: null, values: null, protocol: null, host: null, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(this NavigationHelper helper, string action, object values)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller: null, values: values, protocol: null, host: null, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(this NavigationHelper helper, string action, string controller)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller, values: null, protocol: null, host: null, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(this NavigationHelper helper, string action, string controller, object values)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller, values, protocol: null, host: null, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(
            this NavigationHelper helper,
            string action,
            string controller,
            object values,
            string protocol)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller, values, protocol, host: null, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(
            this NavigationHelper helper,
            string action,
            string controller,
            object values,
            string protocol,
            string host)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(action, controller, values, protocol, host, fragment: null);
        }

        public static RouteValueDictionary GetReturnRoute(
            this NavigationHelper helper,
            string action,
            string controller,
            object values,
            string protocol,
            string host,
            string fragment)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            return helper.GetReturnRoute(new UrlActionContext()
            {
                Action = action,
                Controller = controller,
                Host = host,
                Values = values,
                Protocol = protocol,
                Fragment = fragment
            });
        }
    }
}
