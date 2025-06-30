using System;

namespace SilverLeaf.Entities.DTOs
{
    public class CourseDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime IntroducedOn { get; set; }
    }
}
