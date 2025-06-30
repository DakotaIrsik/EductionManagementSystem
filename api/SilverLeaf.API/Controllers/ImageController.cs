using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.API.Interfaces;
using System.Threading.Tasks;
using SilverLeaf.Common.Models;

namespace SilverLeaf.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {
        private readonly IEMSGeneralAPI _generalApi;
        public ImageController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<ImageController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IEMSGeneralAPI generalApi) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            _generalApi = generalApi;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<ApplicationFile>> Upload(string fileType, string subType)
        {
            var file = Request.Form.Files[0];
            return await _generalApi.UploadImage(Settings.Name, 
                                                 fileType, 
                                                 subType, 
                                                 new Refit.StreamPart(file.OpenReadStream(), file.FileName)).ConfigureAwait(false);
        }
    }
}

