using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OralScreenerController : BaseController
    {
        private IOralScreenerDomain OralScreener { get; set; }
        public OralScreenerController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<PendingScreenerController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IOralScreenerDomain oralScreener) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            OralScreener = oralScreener;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OralScreenerDTO>), 200)]
        public async Task<ActionResult<List<OralScreenerDTO>>> GetAsync()
        {
            var states = await Cache.GetOrSetAsync(
                async () => await OralScreener.GetAsync().ConfigureAwait(false),
                new { Key = "Screener" },
                Settings.Timers.Caches.Default
            ).ConfigureAwait(false);
            return GetCustomResponse(states);
        }

        [HttpPost("Incomplete")]
        [ProducesResponseType(typeof(List<OralScreenerDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<List<OralScreenerDTO>>> IncompleteByStudent(StudentQuestionsRequest request)
        {
            var state = await OralScreener.GetIncompleteByStudentAsync(request.StudentId, request.PagingRequest).ConfigureAwait(false);
            return GetCustomResponse(state);
        }

        [HttpPut]
        [ProducesResponseType(typeof(OralScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<OralScreenerDTO>> SubmitResponse(OralScreenerSubmissionRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dto = Mapper.Map<OralScreenerDTO>(model);
            var result = await OralScreener.SubmitResponseAsync(dto).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpPut("Bulk")]
        [ProducesResponseType(typeof(OralScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<OralScreenerDTO>> BulkSubmitResponses(List<OralScreenerSubmissionRequest> model)
        {
            var mapped = Mapper.Map<List<OralScreenerDTO>>(model);
            var result = await OralScreener.SubmitBulkResponseAsync(mapped);
            return GetCustomResponse(result);
        }
    }
}