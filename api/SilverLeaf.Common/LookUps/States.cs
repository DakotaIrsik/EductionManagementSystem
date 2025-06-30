using System.Collections.Generic;

namespace SilverLeaf.Common.LookUps
{
    public static class States
    {
        public static List<State> ToList => new List<State>
        {
            new State { Id = 1, Name = "Alabama", Abbreviation = "AL" },
            new State { Id = 2, Name = "Alaska", Abbreviation = "AK" },
            new State { Id = 4, Name = "Arizona", Abbreviation = "AZ" },
            new State { Id = 5, Name = "Arkansas", Abbreviation = "AR" },
            new State { Id = 6, Name = "California", Abbreviation = "CA" },
            new State { Id = 7, Name = "Colorado", Abbreviation = "CO" },
            new State { Id = 8, Name = "Connecticut", Abbreviation = "CT" },
            new State { Id = 9, Name = "Delaware", Abbreviation = "DE" },
            new State { Id = 12, Name = "Florida", Abbreviation = "FL" },
            new State { Id = 13, Name = "Georgia", Abbreviation = "GA" },
            new State { Id = 15, Name = "Hawaii", Abbreviation = "HI" },
            new State { Id = 16, Name = "Idaho", Abbreviation = "ID" },
            new State { Id = 17, Name = "Illinois", Abbreviation = "IL" },
            new State { Id = 18, Name = "Indiana", Abbreviation = "IN" },
            new State { Id = 19, Name = "Iowa", Abbreviation = "IA" },
            new State { Id = 20, Name = "Kansas", Abbreviation = "KS" },
            new State { Id = 21, Name = "Kentucky", Abbreviation = "KY" },
            new State { Id = 22, Name = "Lousiana", Abbreviation = "LA" },
            new State { Id = 23, Name = "Maine", Abbreviation = "ME" },
            new State { Id = 25, Name = "Maryland", Abbreviation = "MD" },
            new State { Id = 26, Name = "Massachusetts", Abbreviation = "MA" },
            new State { Id = 27, Name = "Michigan", Abbreviation = "MI" },
            new State { Id = 28, Name = "Minnesota", Abbreviation = "MN" },
            new State { Id = 29, Name = "Mississippi", Abbreviation = "MS" },
            new State { Id = 30, Name = "Missouri", Abbreviation = "MO" },
            new State { Id = 31, Name = "Montana", Abbreviation = "MT" },
            new State { Id = 32, Name = "Nebraska", Abbreviation = "NE" },
            new State { Id = 33, Name = "Nevada", Abbreviation = "NV" },
            new State { Id = 34, Name = "New Hampshire", Abbreviation = "NH" },
            new State { Id = 35, Name = "New Jersey", Abbreviation = "NJ" },
            new State { Id = 36, Name = "New Mexico", Abbreviation = "NM" },
            new State { Id = 37, Name = "New York", Abbreviation = "NY" },
            new State { Id = 38, Name = "North Carolina", Abbreviation = "NC" },
            new State { Id = 39, Name = "North Dakota", Abbreviation = "ND" },
            new State { Id = 41, Name = "Ohio", Abbreviation = "OH" },
            new State { Id = 42, Name = "Oklahoma", Abbreviation = "OK" },
            new State { Id = 43, Name = "Oregon", Abbreviation = "OR" },
            new State { Id = 44, Name = "Pennsyylvania", Abbreviation = "PA" },
            new State { Id = 45, Name = "Peurto Rico", Abbreviation = "PR" },
            new State { Id = 46, Name = "Rhode Island", Abbreviation = "RI" },
            new State { Id = 47, Name = "South Carolina", Abbreviation = "SC" },
            new State { Id = 48, Name = "South Dakota", Abbreviation = "SD" },
            new State { Id = 49, Name = "Tennessee", Abbreviation = "TN" },
            new State { Id = 50, Name = "Texas", Abbreviation = "TX" },
            new State { Id = 51, Name = "Utah", Abbreviation = "UT" },
            new State { Id = 52, Name = "Vermont", Abbreviation = "VT" },
            new State { Id = 53, Name = "Virgin Island", Abbreviation = "VI" },
            new State { Id = 54, Name = "Virginia", Abbreviation = "VA" },
            new State { Id = 55, Name = "Washington", Abbreviation = "WA" },
            new State { Id = 56, Name = "West Virginia", Abbreviation = "WV" },
            new State { Id = 57, Name = "Wisconsin", Abbreviation = "WI" },
            new State { Id = 58, Name = "Wyoming", Abbreviation = "WY" }
        };
    }


    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}
