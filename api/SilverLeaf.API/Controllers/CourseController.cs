using AutoMapper;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseController : BaseController
    {
        private readonly ICourseDomain _course;
        public CourseController(ICourseDomain courseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<CourseController> logger,
                                IMapper mapper,
                                ICacheService cache) : base((IBaseDomain)courseDomain, configuration, httpContext, logger, mapper, cache)
        {
            _course = courseDomain;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdjustableDTO<CourseDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]

        public async Task<ActionResult> Get(bool useNoSql = true)
        {
            var result = await _course.GetAsync(useNoSql).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> Get(int id, bool useNoSql = true)
        {
            var result = await _course.GetAsync(id, useNoSql).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [ProducesResponseType(typeof(CourseDTO), 201)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        [HttpPost("Create")]
        public async Task<ActionResult<CourseDTO>> Create(CourseDTO inputData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _course.CreateOrUpdateAsync(inputData).ConfigureAwait(false);
            return GetCustomResponse(result, null, new Uri(GetCreatedLink(result.Id.ToString(CultureInfo.InvariantCulture))));
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _course.SoftDeleteAsync(id).ConfigureAwait(false);
            return NoContent();
        }
    }
}

