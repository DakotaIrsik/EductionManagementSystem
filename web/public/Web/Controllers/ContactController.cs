using Microsoft.AspNetCore.Mvc;
using SilverLeaf.Public.Web.Models;
using System.Net;
using System.Net.Mail;

namespace SilverLeaf.Public.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact([Bind("Name,Phone,Email,Description")] ContactUsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.office365.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("admin@silverleafschool.com", "Shield22@@");
                var message = new MailMessage("admin@silverleafschool.com", "admin@silverleafschool.com, dakotairsik@gmail.com, 516567437@qq.com, cameron514@gmail.com, reinilda.blair@gmail.com", "New Contact Request : " + viewModel.Name, GetLogEntryText(viewModel));
                client.Send(message);
                return RedirectToAction(nameof(Confirmation));
            }

            return RedirectToAction(nameof(Index));
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
