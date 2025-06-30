using Microsoft.AspNetCore.Mvc;

namespace SilverLeaf.Public.Web.Controllers
{
    public class PrivacyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
