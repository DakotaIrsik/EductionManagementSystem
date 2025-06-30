using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SilverLeaf.Public.Web.Common;

namespace SilverLeaf.Public.Web.Models
{
    public class DemoFeedbackViewModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        [Alphanumeric(ErrorMessage = "You may only enter Alphanumeric characters.")]
        [DisplayName("Name")]
        public string Name { get; set; }

        public List<InformationPoint> InformationPoints { get; set; }

        public string WhatDidYourChildLike { get; set; }
        public string WhatCouldWeDoBetter { get; set; }

        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }
        public string Question6 { get; set; }
        public string Question7 { get; set; }
        public string Question8 { get; set; }
    }


    public class InformationPoint
    {
        public string QuestionId { get; set; }
        public string Question { get; set; }
        public List<SelectListItem> Answers { get; set; }
        public SelectListItem Answer { get; set; }
        public string Extra { get; set; }
    }
}
