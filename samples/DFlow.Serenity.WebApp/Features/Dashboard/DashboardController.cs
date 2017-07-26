using Microsoft.AspNetCore.Mvc;

namespace DFlow.Serenity.WebApp.Features.Dashboard
{
    //[Route("Dashboard/[action]")]
    public class DashboardController : Controller
    {
        [Route("~/")]
        public ActionResult Index()
        {
            return View(new DashboardPageModel { OpenOrders = 21, ClosedOrderPercent = 97, CustomerCount = 91, ProductCount = 77 });
        }
    }
}
