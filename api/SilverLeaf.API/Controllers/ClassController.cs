using AutoMapper;
using CacheExtensions;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClassController : BaseController
    {
        private readonly IClassDomain _class;
        public ClassController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<ClassController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IClassDomain classDomain) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            _class = classDomain;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AdjustableDTO<ClassDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<ClassDTO>>> GetAsync()
        {
            var classes = await Cache.GetOrSetAsync(
                async () => await _class.GetAsync().ConfigureAwait(false),
                new { Key = "Class" },
                Settings.Timers.Apis.General).ConfigureAwait(false);
            return GetCustomResponse(classes);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AdjustableDTO<ClassDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<AdjustableDTO<ClassDTO>>> SearchAsync(ClassSearchRequest request, bool useNoSql = true)
        {
            var classes = await Cache.GetOrSetAsync(
                async () => await _class.SearchAsync(request, useNoSql).ConfigureAwait(false),
                new { Key = "Classes" },
                Settings.Timers.Apis.General).ConfigureAwait(false);
            return GetCustomResponse(classes, request?.Fields);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<ClassDTO>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult<List<ClassDTO>>> GetByIdAsync(int id)
        {
            var classes = await Cache.GetOrSetAsync(
                async () => await _class.GetAsync(id).ConfigureAwait(false),
                new { Key = $"Class{id}" },
                Settings.Timers.Apis.General).ConfigureAwait(false);
            return GetCustomResponse(classes);
        }
    }
}