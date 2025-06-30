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
    public class PhonicsScreenerController : BaseController
    {
        private IPhonicsScreenerDomain PhonicsScreener { get; set; }
        public PhonicsScreenerController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<PendingScreenerController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IPhonicsScreenerDomain phonicsScreener) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            PhonicsScreener = phonicsScreener;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PhonicsScreenerDTO>), 200)]
        public async Task<ActionResult<List<PhonicsScreenerDTO>>> GetAsync()
        {
            var states = await Cache.GetOrSetAsync(
                async () => await PhonicsScreener.GetAsync().ConfigureAwait(false),
                new { Key = "Screener" },
                Settings.Timers.Caches.Default
            ).ConfigureAwait(false);
            return GetCustomResponse(states);
        }

        [HttpPost("Incomplete")]
        [ProducesResponseType(typeof(List<PhonicsScreenerDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<List<PhonicsScreenerDTO>>> IncompleteByStudent(StudentQuestionsRequest request)
        {
            var state = await PhonicsScreener.GetIncompleteByStudentAsync(request.StudentId, request.PagingRequest).ConfigureAwait(false);
            return GetCustomResponse(state);
        }

        [HttpPut]
        [ProducesResponseType(typeof(PhonicsScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<PhonicsScreenerDTO>> SubmitResponse(PhonicsScreenerSubmissionRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dto = Mapper.Map<PhonicsScreenerDTO>(model);
            var result = await PhonicsScreener.SubmitResponseAsync(dto).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpPut("Bulk")]
        [ProducesResponseType(typeof(PhonicsScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<PhonicsScreenerDTO>> BulkSubmitResponses(List<PhonicsScreenerSubmissionRequest> model)
        {
            var mapped = Mapper.Map<List<PhonicsScreenerDTO>>(model);
            var result = await PhonicsScreener.SubmitBulkResponseAsync(mapped);
            return GetCustomResponse(result);
        }
    }
}