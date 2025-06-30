using Nest;

namespace SilverLeaf.Entities.ElasticSearch
{
    public class ElasticClass : BaseElastic
    {
        public static CreateIndexDescriptor IndexDescriptor => new CreateIndexDescriptor("class").Map<ElasticClass>(m => m.AutoMap());

        public int CourseId { get; set; }

        public ElasticCourse Course { get; set; }

        public int Lesson { get; set; }

        public int Week { get; set; }

        [Text]
        public string Session { get; set; }

        [Text]
        public string Card { get; set; }

        [Text]
        public string Fictionality { get; set; }

        [Text]
        public string Genre { get; set; }

        [Text]
        public string Title { get; set; }

        [Text]
        public string TargetReadingSkill { get; set; }

        public string Classwork { get; set; }

        public string Homework { get; set; }

        public string Material { get; set; }

        public string LessonPlan { get; set; }

        public string Flipchart { get; set; }

        public string AllDocuments { get; set; }

        [Text]
        public string Phonics { get; set; }
    }
}
