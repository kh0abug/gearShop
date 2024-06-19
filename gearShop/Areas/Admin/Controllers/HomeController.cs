using GearShopWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GearShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
