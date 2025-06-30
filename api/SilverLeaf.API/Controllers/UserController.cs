using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Core.BusinessLogic;
using System.Threading.Tasks;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Entities.DTOs;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserDomain _userDomain;

        public UserController(IUserDomain userDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<UserController> logger,
                                IMapper mapper,
                                ICacheService cache) : base((IBaseDomain)userDomain, configuration, httpContext, logger, mapper, cache)
        {
            _userDomain = userDomain;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<UserDTO> CreateOrUpdateUser()
        {
            return new UserDTO()
            {
                IdentityUserId = "1",
                UserName = "Hangzhou"
            };
            //wait _userDomain.CreateOrUpdateUser().ConfigureAwait(false);
        }
    }
}