using Refit;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.LookUps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Interfaces
{
    public interface ISeedEMSGeneralAPI
    {
        #region City
        [Post("/api/v1/City")]
        Task<List<City>> GetCities([Body] GeoDbPagingRequest request);
        #endregion

        #region State
        [Get("/api/v1/State")]
        Task<List<State>> GetStates();

        [Get("/api/v1/State/{id}")]
        Task<Dictionary<string, string>> GetState(string id);
        #endregion

        #region Country
        [Get("/api/v1/Country")]
        Task<List<State>> GetCountries();

        [Get("/api/v1/Country/{id}")]
        Task<Dictionary<string, string>> GetCountry(string id);
        #endregion

        #region Images
        [Post("/api/v1/Image?applicationName={applicationName}&fileType={fileType}&subType={subType}")]
        [Multipart]
        Task<ApplicationFile> UploadImage(string applicationName, string fileType, string subType, [AliasAs("imageContent")] StreamPart imageStream);
        #endregion

    }
}