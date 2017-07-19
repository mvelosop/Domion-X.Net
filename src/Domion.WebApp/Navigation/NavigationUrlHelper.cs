using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domion.WebApp.Navigation
{
    public class NavigationUrlHelper : UrlHelper
    {
        private readonly ActionContext Context;

        public NavigationUrlHelper(ActionContext context)
            : base(context)
        {
            Context = context;
        }

        public RouteValueDictionary ReturnRoute(string action)
        {

            throw new NotImplementedException();
        }
    }
}
