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
    public class FeedbackController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFeedbackDomain _feedbackDomain;

        public FeedbackController(IFeedbackDomain feedbackDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<StudentController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IWebHostEnvironment hostingEnvironment) : base((IBaseDomain)feedbackDomain, configuration, httpContext, logger, mapper, cache)
        {
            _feedbackDomain = feedbackDomain;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [ProducesResponseType(typeof(StudentDTO), 201)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public ActionResult<FeedbackDTO> Create(FeedbackRequest inputData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dto = Mapper.Map<FeedbackDTO>(inputData);
            var result = _feedbackDomain.CreateOrUpdate(dto);
            return GetCustomResponse(result);
        }


        [HttpPost("Search")]
        [ProducesResponseType(typeof(List<FeedbackDTO>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public async Task<ActionResult> Search(PagingRequest searchRequest)
        {
            var result = await _feedbackDomain.Get(searchRequest).ConfigureAwait(false);
            return GetCustomResponse(result, searchRequest?.Fields);
        }
    }
}

