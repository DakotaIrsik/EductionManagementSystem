using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilverLeaf.Public.Web.Services;
using System.Collections.Generic;

namespace SilverLeaf.Public.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly LocalizationService _localizationService;
        public BaseController(LocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        protected List<SelectListItem> Satisfaction => new List<SelectListItem>
        {
            new SelectListItem(_localizationService.GetLocalizedHtmlString("SatisfactionVerySatisfied"), "Very Satisfied"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("SatisfactionSatisfied"), "Satisfied"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("SatisfactionAverage"), "Average"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("SatisfactionUnsatisfied"), "Unsatisfied")
        };

        protected List<SelectListItem> Agreement => new List<SelectListItem>
        {
            new SelectListItem(_localizationService.GetLocalizedHtmlString("AgreementStronglyAgree"), "Strongly Agree"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("AgreementAgree"), "Agree"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("AgreementPartiallyAgree"), "Partially Agree"),
            new SelectListItem(_localizationService.GetLocalizedHtmlString("AgreementDisagree"), "Disagree")
        };


    }
}
