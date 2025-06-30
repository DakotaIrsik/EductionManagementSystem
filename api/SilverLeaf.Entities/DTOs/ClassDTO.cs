namespace SilverLeaf.Entities.DTOs
{
    public class ClassDTO : BaseDTO
    {
        public int CourseId { get; set; }

        public CourseDTO Course { get; set; }

        public int Lesson { get; set; }

        public int Week { get; set; }

        public string Session { get; set; }

        public string Card { get; set; }

        public string Fictionality { get; set; }

        public string Genre { get; set; }

        public string Title { get; set; }

        public string TargetReadingSkill { get; set; }

        public string Classwork { get; set; }

        public string Homework { get; set; }

        public string LessonPlan { get; set; }

        public string Flipchart { get; set; }

        public string Material { get; set; }

        public string AllDocuments { get; set; }

        public string Phonics { get; set; }
    }
}
