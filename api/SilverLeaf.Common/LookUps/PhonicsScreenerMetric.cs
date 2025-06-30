using System.Collections.Generic;
using System.Linq;

namespace SilverLeaf.Common.LookUps
{
    public class PhonicsTask
    {
        public PhonicsTask(int courseId, int task, int passThreshhold)
        {
            CourseId = courseId;
            Task = task;
            PassThreshhold = passThreshhold;
        }

        public int CourseId { get; set; }

        public int Task { get; set; }

        public int PassThreshhold { get; set; }

        public int Correct { get; set; }

        public int Total { get; set; }
    }

    public class PhonicsMetrics //: IPhonicsScreenerMetric
    {

        public List<PhonicsTask> PhonicsTasks { get; set; } = new List<PhonicsTask>();

        public int TotalCorrect(int courseId)
        {
            return PhonicsTasks.Where(t => t.CourseId == courseId).Sum(t => t.Correct);
        }

    }
}
