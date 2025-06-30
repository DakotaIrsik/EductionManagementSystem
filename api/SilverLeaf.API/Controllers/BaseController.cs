using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Extensions;
using SilverLeaf.Core.BusinessLogic;
using System;
using Newtonsoft.Json;

namespace SilverLeaf.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IHttpContextAccessor Http { get; set; }
        protected IMapper Mapper { get; set; }
        protected ICacheService Cache { get; set; }
        protected AppSettings Settings { get; set; }
        protected ILogger Logger { get; set; }


        protected IBaseDomain Domain { get; set; }
        public BaseController(IBaseDomain domain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger logger,
                                IMapper mapper,
                                ICacheService cache)
        {
            Domain = domain;
            Http = httpContext;
            Mapper = mapper;
            Cache = cache;
            Settings = configuration?.Value;
            Logger = logger;
        }

        protected ActionResult GetCustomResponse(object o, string fields = null, Uri uri = null)
        {
            if (Domain.HasErrors == true)
            {
                return BadRequest(Domain.GetErrors());
            }
            if (o == null)
            {
                return NotFound();
            }
            else if (uri != null)
            {
                return Created(uri, o.FieldSelect(fields));
            }
            else
            {
                var x = o.FieldSelect(fields);
                var y = JsonConvert.SerializeObject(x, Formatting.Indented);
                return Ok(y);
            }
        }

        protected string GetCreatedLink(string id)
        {
            var request = Http.HttpContext.Request;
            UriBuilder uriBuilder = new UriBuilder();
            if (request.Host.Port.HasValue && request.Host.Port != 80 && request.Host.Port != 443)
            {
                uriBuilder.Port = request.Host.Port.Value;
            }
            uriBuilder.Scheme = request.Scheme;
            uriBuilder.Host = request.Host.Host;
            uriBuilder.Path = request.Path.ToString();
            uriBuilder.Query = request.QueryString.ToString();
            return $"{uriBuilder.Uri}/{id}";
        }
    }
}