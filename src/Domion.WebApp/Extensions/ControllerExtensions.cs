using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domion.WebApp.Extensions
{
    public static class ControllerExtensions
    {
        public static void AlertDanger(this Controller controller, string message, params object[] args)
        {
            cloudscribe.Web.Common.Extensions.ControllerExtensions.AlertDanger(controller, string.Format(message, args));
        }

        public static void AlertInformation(this Controller controller, string message, params object[] args)
        {
            cloudscribe.Web.Common.Extensions.ControllerExtensions.AlertInformation(controller, string.Format(message, args));
        }

        public static void AlertSuccess(this Controller controller, string message, params object[] args)
        {
            cloudscribe.Web.Common.Extensions.ControllerExtensions.AlertSuccess(controller, string.Format(message, args));
        }

        public static void AlertWarning(this Controller controller, string message, params object[] args)
        {
            cloudscribe.Web.Common.Extensions.ControllerExtensions.AlertWarning(controller, string.Format(message, args));
        }


    }
}
