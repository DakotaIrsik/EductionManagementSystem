using AutoMapper;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TeacherController : BaseController
    {
        public TeacherController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<TeacherController> logger,
                                IMapper mapper,
                                ICacheService cache) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TeacherDTO>), 200)]
        public ActionResult<List<TeacherDTO>> Get()
        {
            return GetCustomResponse(new List<TeacherDTO>());
        }

        //[HttpGet("{id}")]
        //[ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        //[ProducesResponseType(typeof(object), 404)]
        //public ActionResult<Dictionary<string, string>> ById(string id)
        //{
        //    return GetCustomResponse(new List<TeacherDTO>());
        //}
    }
}