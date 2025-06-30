using SilverLeaf.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverLeaf.Entities.Models
{
    [Table("Class")]
    public class Class : Trackable
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int Lesson { get; set; }

        public int Week { get; set; }

        public string Session { get; set; }

        public string Card { get; set; }

        public string Fictionality { get; set; }

        public string TargetReadingSkill { get; set; }

        public string Genre { get; set; }

        public string Title { get; set; }

        public string Classwork { get; set; }

        public string Homework { get; set; }

        public string LessonPlan { get; set; }

        public string Material { get; set; }

        public string Flipchart { get; set; }

        public string AllDocuments { get; set; }

        public string Phonics { get; set; }

        public IList<ClassStaff> ClassStaff { get; set; }
    }
}
