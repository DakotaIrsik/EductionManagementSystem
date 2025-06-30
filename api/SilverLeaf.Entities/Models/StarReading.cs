using SilverLeaf.Common;

namespace SilverLeaf.Entities.Models
{
    public class StarReading : Trackable
    {
        public int StudentId { get; set; }

        public Student Student { get; set; }

        public double GradeEquivalentLevel { get; set; } = 0.0;

        public double InstructionalReadingLevel { get; set; } = 0.0;

        public double ZoneOfProximalDevelopment { get; set; } = 0.0;

        public string TimeTaken { get; set; } = "N/A";

        public int PracticeQuestionsAnswered { get; set; } = 0;
    }
}
