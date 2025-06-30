using System.Collections.Generic;
using System.Linq;

namespace SilverLeaf.Common.Models
{
    public class Error
    {
        public Error(string propertyName, string error)
        {
            var existingPropertyError = Errors.FirstOrDefault(e => PropertyName == propertyName);
            if (existingPropertyError == null)
            {
                PropertyName = propertyName;
            }

            Errors.Add(error);
        }


        public string PropertyName { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
