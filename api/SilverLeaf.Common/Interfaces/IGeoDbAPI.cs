using Refit;
using SilverLeaf.Common.LookUps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Interfaces
{
    public interface IGeoDbAPI
    {

        [Get("/v1/geo/countries/US/regions/{state}/cities?minPopulation=25000&limit={size}")]
        Task<GeoDbResponse<IEnumerable<City>>> GetCities([Header("X-RapidAPI-Key")] string authorization, string state, int size);
    }

    public class GeoDbResponse<T>
    {
        public T Data { get; set; }
    }
}
