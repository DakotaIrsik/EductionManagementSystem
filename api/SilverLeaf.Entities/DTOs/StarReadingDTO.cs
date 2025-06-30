namespace SilverLeaf.Entities.DTOs
{
    public class StarReadingDTO
    {
        public double GradeEquivalentLevel { get; set; } = 0.0;

        public double InstructionalReadingLevel { get; set; } = 0.0;

        public double ZoneOfProximalDevelopment { get; set; } = 0.0;

        public string TimeTaken { get; set; } = "N/A";

        public int PracticeQuestionsAnswered { get; set; } = 0;
    }
}
