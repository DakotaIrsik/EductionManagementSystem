using AutoMapper;
using CacheExtensions;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.LookUps;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComprehensionScreenerController : BaseController
    {
        private IComprehensionScreenerDomain ComprehensionScreener { get; set; }
        public ComprehensionScreenerController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<PendingScreenerController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IComprehensionScreenerDomain comprehensionScreener) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            ComprehensionScreener = comprehensionScreener;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ComprehensionScreenerDTO>), 200)]
        public async Task<ActionResult<List<ComprehensionScreenerDTO>>> GetAsync()
        {
            var states = await Cache.GetOrSetAsync(
                async () => await ComprehensionScreener.GetAsync().ConfigureAwait(false),
                new { Key = "Screener" },
                Settings.Timers.Caches.Default
            ).ConfigureAwait(false);
            return GetCustomResponse(states);
        }

        [HttpPost("Incomplete")]
        [ProducesResponseType(typeof(List<ComprehensionScreenerDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<List<ComprehensionScreenerDTO>>> IncompleteByStudent(StudentQuestionsRequest request)
        {
            var state = await ComprehensionScreener.GetIncompleteByStudentAsync(request.StudentId, request.PagingRequest).ConfigureAwait(false);
            return GetCustomResponse(state);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ComprehensionScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<ComprehensionScreenerDTO>> SubmitResponse(ComprehensionScreenerSubmissionRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dto = Mapper.Map<ComprehensionScreenerDTO>(model);
            var result = await ComprehensionScreener.SubmitResponseAsync(dto).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpPut("Bulk")]
        [ProducesResponseType(typeof(ComprehensionScreenerDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<ComprehensionScreenerDTO>> BulkSubmitResponses(List<ComprehensionScreenerSubmissionRequest> model)
        {
            var mapped = Mapper.Map<List<ComprehensionScreenerDTO>>(model);
            var result = await ComprehensionScreener.SubmitBulkResponseAsync(mapped);
            return GetCustomResponse(result);
        }
    }
}