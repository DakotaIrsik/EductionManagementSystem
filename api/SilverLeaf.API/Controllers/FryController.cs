using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class FryController : BaseController
    {
        private readonly IFryDomain _fryDomain;

        public FryController(IFryDomain fryDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<StudentController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IWebHostEnvironment hostingEnvironment) : base((IBaseDomain)fryDomain, configuration, httpContext, logger, mapper, cache)
        {
            _fryDomain = fryDomain;
        }


        [HttpPost("Search")]
        [ProducesResponseType(typeof(List<FryDTO>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public async Task<ActionResult> Search(PagingRequest searchRequest)
        {
            var result = await _fryDomain.Get(searchRequest).ConfigureAwait(false);
            return GetCustomResponse(result, searchRequest?.Fields);
        }
    }
}

