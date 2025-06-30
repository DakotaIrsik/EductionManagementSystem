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
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class PhonicsSkillController : BaseController
    {
        private readonly IPhonicsSkillDomain _phonicsSkillDomain;

        public PhonicsSkillController(IPhonicsSkillDomain phonicsSkillDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<StudentController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IWebHostEnvironment hostingEnvironment) : base((IBaseDomain)phonicsSkillDomain, configuration, httpContext, logger, mapper, cache)
        {
            _phonicsSkillDomain = phonicsSkillDomain;
        }


        [HttpPost("Search")]
        [ProducesResponseType(typeof(List<PhonicsSkillDTO>), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public async Task<ActionResult> Search(PagingRequest searchRequest)
        {
            var result = await _phonicsSkillDomain.Get(searchRequest).ConfigureAwait(false);
            return GetCustomResponse(result, searchRequest?.Fields);
        }
    }
}

