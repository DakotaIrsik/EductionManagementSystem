using System.Collections.Generic;

namespace SilverLeaf.Common.LookUps
{
    public static class Countries
    {
        public static List<Country> ToList => new List<Country>
        {
            new Country { Id = 1, Name = "United States", Abbreviation = "USA" },
            new Country { Id = 2, Name = "United Kingdom", Abbreviation = "UK" },
            new Country { Id = 4, Name = "Japan", Abbreviation = "JP" },
            new Country { Id = 5, Name = "Austrailia", Abbreviation = "AU" },
            new Country { Id = 6, Name = "China", Abbreviation = "CH" }
        };
    }


    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
