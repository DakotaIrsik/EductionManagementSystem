using AutoMapper;
using CacheExtensions;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using Microsoft.AspNetCore.Hosting;
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
    public class StudentController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IStudentDomain _student;
        public StudentController(IStudentDomain studentDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<StudentController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                IWebHostEnvironment hostingEnvironment) : base((IBaseDomain)studentDomain, configuration, httpContext, logger, mapper, cache)
        {
            _student = studentDomain;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDTO), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> Get(int id, bool useNoSql = true)
        {
            var result = await _student.GetAsync(id, useNoSql).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpPost("GetAsync")]
        [ProducesResponseType(typeof(AdjustableDTO<StudentDTO>), 200)]
        public async Task<ActionResult<AdjustableDTO<StudentDTO>>> GetAsync(PagingRequest pagingRequest)
        {
            var classes = await Cache.GetOrSetAsync(
                async () => await _student.GetAsync(pagingRequest).ConfigureAwait(false),
                new { Key = "AllStudents" },
                Settings.Timers.Apis.General).ConfigureAwait(false);
            return GetCustomResponse(classes);
        }

        [HttpPost("MyStudents")]
        [ProducesResponseType(typeof(AdjustableDTO<IEnumerable<StudentDTO>>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> MyStudents(PagingRequest request, bool useNoSql = true)
        {
            var result = await _student.MyStudentsAsync(request, useNoSql).ConfigureAwait(false);
            return GetCustomResponse(result, request?.Fields);
        }

        [HttpPost("Search")]
        [ProducesResponseType(typeof(AdjustableDTO<IEnumerable<StudentDTO>>), 200)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> Search(StudentSearchRequest searchRequest, bool useNoSql = true)
        {
            var result = await _student.SearchAsync(searchRequest, useNoSql).ConfigureAwait(false);
            return GetCustomResponse(result, searchRequest?.Fields);
        }

        [HttpPost("CreateStudent")]
        [ProducesResponseType(typeof(StudentDTO), 201)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public async Task<ActionResult<StudentDTO>> CreateStudent(StudentRegistrationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _student.CreateOrUpdateAsync(Mapper.Map<StudentDTO>(model)).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(StudentDTO), 200)]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), 400)]
        public ActionResult<StudentDTO> UpdateStudent(StudentDTO inputData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _student.CreateOrUpdateAsync(inputData).ConfigureAwait(false);
            return GetCustomResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _student.SoftDeleteAsync(id).ConfigureAwait(false);
            return NoContent();
        }
    }
}

