using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PendingScreenerController : BaseController
    {
        private IPendingScreenerDomain _pendingScreeners { get; set; }
        public PendingScreenerController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<PendingScreenerController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IPendingScreenerDomain pendingScreeners) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            _pendingScreeners = pendingScreeners;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<StudentDTO>), 200)]
        public async Task<ActionResult<List<StudentDTO>>> GetPendingScreeners(PendingScreenerSearchRequest pagingRequest)
        {
            var result = await Cache.GetOrSetAsync(
                async () => await _pendingScreeners.GetAsync(pagingRequest).ConfigureAwait(false),
                new { Key = "PendingScreeners" },
                Settings.Timers.Caches.Default
            ).ConfigureAwait(false);
            return GetCustomResponse(result, pagingRequest.Fields);
        }
    }
}