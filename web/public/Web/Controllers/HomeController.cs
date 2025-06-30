using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SilverLeaf.Public.Web.Models;
using System.Diagnostics;

namespace SilverLeaf.Public.Web.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 30)]
        public IActionResult Index()
        {
            return View();
        }

       


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetLogEntryText(ContactUsViewModel model)
        {
            string name = @"Name : " + model.Name + " \r\n -------------------------------------------------\r\n";
            string phone = @"Phone Number : " + model.Phone + " \r\n -------------------------------------------------\r\n";
            string email = @"Email : " + model.Email + " \r\n -------------------------------------------------\r\n";
            string description = @"Description : " + model.Description + " \r\n -------------------------------------------------\r\n\r\n";
            return name + phone + email + description;
        }
    }
}
