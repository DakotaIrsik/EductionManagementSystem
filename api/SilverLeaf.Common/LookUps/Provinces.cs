using System.Collections.Generic;

namespace SilverLeaf.Common.LookUps
{
    public static class Provinces
    {
        public static List<Province> ToList => new List<Province>
        {
            new Province { Id = 1, Name = "Alabama", Abbreviation = "AL" },
            new Province { Id = 2, Name = "Alaska", Abbreviation = "AK" },
            new Province { Id = 4, Name = "Arizona", Abbreviation = "AZ" },
            new Province { Id = 5, Name = "Arkansas", Abbreviation = "AR" },
            new Province { Id = 6, Name = "California", Abbreviation = "CA" },
            new Province { Id = 7, Name = "Colorado", Abbreviation = "CO" },
            new Province { Id = 8, Name = "Connecticut", Abbreviation = "CT" },
            new Province { Id = 9, Name = "Delaware", Abbreviation = "DE" },
            new Province { Id = 12, Name = "Florida", Abbreviation = "FL" },
            new Province { Id = 13, Name = "Georgia", Abbreviation = "GA" },
            new Province { Id = 15, Name = "Hawaii", Abbreviation = "HI" },
            new Province { Id = 16, Name = "Idaho", Abbreviation = "ID" },
            new Province { Id = 17, Name = "Illinois", Abbreviation = "IL" },
            new Province { Id = 18, Name = "Indiana", Abbreviation = "IN" },
            new Province { Id = 19, Name = "Iowa", Abbreviation = "IA" },
            new Province { Id = 20, Name = "Kansas", Abbreviation = "KS" },
            new Province { Id = 21, Name = "Kentucky", Abbreviation = "KY" },
            new Province { Id = 22, Name = "Lousiana", Abbreviation = "LA" },
            new Province { Id = 23, Name = "Maine", Abbreviation = "ME" },
            new Province { Id = 25, Name = "Maryland", Abbreviation = "MD" },
            new Province { Id = 26, Name = "Massachusetts", Abbreviation = "MA" },
            new Province { Id = 27, Name = "Michigan", Abbreviation = "MI" },
            new Province { Id = 28, Name = "Minnesota", Abbreviation = "MN" },
            new Province { Id = 29, Name = "Mississippi", Abbreviation = "MS" },
            new Province { Id = 30, Name = "Missouri", Abbreviation = "MO" },
            new Province { Id = 31, Name = "Montana", Abbreviation = "MT" },
            new Province { Id = 32, Name = "Nebraska", Abbreviation = "NE" },
            new Province { Id = 33, Name = "Nevada", Abbreviation = "NV" },
            new Province { Id = 34, Name = "New Hampshire", Abbreviation = "NH" },
            new Province { Id = 35, Name = "New Jersey", Abbreviation = "NJ" },
            new Province { Id = 36, Name = "New Mexico", Abbreviation = "NM" },
            new Province { Id = 37, Name = "New York", Abbreviation = "NY" },
            new Province { Id = 38, Name = "North Carolina", Abbreviation = "NC" },
            new Province { Id = 39, Name = "North Dakota", Abbreviation = "ND" },
            new Province { Id = 41, Name = "Ohio", Abbreviation = "OH" },
            new Province { Id = 42, Name = "Oklahoma", Abbreviation = "OK" },
            new Province { Id = 43, Name = "Oregon", Abbreviation = "OR" },
            new Province { Id = 44, Name = "Pennsyylvania", Abbreviation = "PA" },
            new Province { Id = 45, Name = "Peurto Rico", Abbreviation = "PR" },
            new Province { Id = 46, Name = "Rhode Island", Abbreviation = "RI" },
            new Province { Id = 47, Name = "South Carolina", Abbreviation = "SC" },
            new Province { Id = 48, Name = "South Dakota", Abbreviation = "SD" },
            new Province { Id = 49, Name = "Tennessee", Abbreviation = "TN" },
            new Province { Id = 50, Name = "Texas", Abbreviation = "TX" },
            new Province { Id = 51, Name = "Utah", Abbreviation = "UT" },
            new Province { Id = 52, Name = "Vermont", Abbreviation = "VT" },
            new Province { Id = 53, Name = "Virgin Island", Abbreviation = "VI" },
            new Province { Id = 54, Name = "Virginia", Abbreviation = "VA" },
            new Province { Id = 55, Name = "Washington", Abbreviation = "WA" },
            new Province { Id = 56, Name = "West Virginia", Abbreviation = "WV" },
            new Province { Id = 57, Name = "Wisconsin", Abbreviation = "WI" },
            new Province { Id = 58, Name = "Wyoming", Abbreviation = "WY" }
        };
    }


    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
