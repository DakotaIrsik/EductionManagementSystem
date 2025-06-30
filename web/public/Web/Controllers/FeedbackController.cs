using Microsoft.AspNetCore.Mvc;
using SilverLeaf.Public.Web.Models;
using SilverLeaf.Public.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SilverLeaf.Public.Web.Controllers
{
    public class FeedbackController : BaseController
    {
        public FeedbackController(LocalizationService localizationService) : base(localizationService)
        {
        }

        public IActionResult Index()
        {
            var model = new DemoFeedbackViewModel
            {
                InformationPoints = GetInformationPoints()
            };

            return View(model);
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Feedback(DemoFeedbackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.office365.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("admin@silverleafschool.com", "Shield22@@");
                var message = new MailMessage("admin@silverleafschool.com", "admin@silverleafschool.com, dakotairsik@gmail.com, 516567437@qq.com, cameron514@gmail.com, reinilda.blair@gmail.com", "New Demo Feedback : " + viewModel.Name, GetLogEntryText(viewModel));
                client.Send(message);
                return RedirectToAction(nameof(Confirmation));
            }

            return RedirectToAction(nameof(Index));
        }

        private List<InformationPoint> GetInformationPoints()
        {
            var informationPoints = new List<InformationPoint>();
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{1}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("ChildParticipationSatisfaction"),
                Answers = Satisfaction,
                Answer = null
            });
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{2}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("ParentalSatisfaction"),
                Answers = Satisfaction,
            });
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{3}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("ParentalSatisfaction"),
                Answers = Satisfaction,
            });
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{4}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("TeacherProfessionallyDressed"),
                Answers = Agreement,
                Answer = null
            });
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{5}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("TeacherProfessionallyDressed"),
                Answers = Agreement,
                Answer = null
            });
            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{6}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("TeacherEngagedWithYourChild"),
                Answers = Agreement,
                Answer = null
            });

            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{7}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("SatisfactionWithPreDemoCommunication"),
                Answers = Satisfaction,
                Answer = null
            });

            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{8}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("SatisfactionWithPreDemoCommunication"),
                Answers = Satisfaction,
                Answer = null
            });

            informationPoints.Add(new InformationPoint
            {
                QuestionId = $"Question{9}",
                Extra = string.Empty,
                Question = _localizationService.GetLocalizedHtmlString("SatisfactionWithArrangements"),
                Answers = Satisfaction,
                Answer = null
            });

            return informationPoints;
        }

        private string GetLogEntryText(DemoFeedbackViewModel model)
        {
            string logEntry = string.Empty;
            logEntry += "Question: " + GetQuestion(1) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(2) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(3) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(4) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(5) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(6) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(7) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            logEntry += "Question: " + GetQuestion(8) + " Answer: " + model.Question1 + " \r\n -------------------------------------------------\r\n";
            return logEntry;
        }

        private string GetQuestion(int questionId)
        {
            var informationPoints = GetInformationPoints();
            return informationPoints.FirstOrDefault(ip => ip.QuestionId == $"Question{questionId}").Question;
        }
    }
}
