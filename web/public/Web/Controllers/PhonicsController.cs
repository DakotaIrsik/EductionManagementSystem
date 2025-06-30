using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilverLeaf.Public.Web.Models;
using System.Diagnostics;

namespace SilverLeaf.Public.Web.Controllers
{
    public class PhonicsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
