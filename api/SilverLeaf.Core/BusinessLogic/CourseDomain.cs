using AutoMapper;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Helpers;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface ICourseDomain
    {
        Task<CourseDTO> CreateOrUpdateAsync(CourseDTO model);
        Task<CourseDTO> GetAsync(int id, bool useNoSql = true);
        Task<AdjustableDTO<CourseDTO>> GetAsync(bool useNoSql = true);
        Task<bool> SoftDeleteAsync(int id);
    }
    public class CourseDomain : BaseDomain, ICourseDomain
    {
        public CourseDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
        }
        
        public async Task<CourseDTO> CreateOrUpdateAsync(CourseDTO model)
        {
            var course = _context.Course.SingleOrDefault(m => m.Id == model.Id);
            if (course == null)
            {
                _context.Course.Add(course);
            }
            else
            {
                course.IsActive = model.IsActive;
            }

            await ValidateAndSaveChangesAsync();
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<CourseDTO> GetAsync(int id, bool useNoSql = true)
        {
            CourseDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticCourse>(new CourseSearchRequest { Id = id }, "course");
                result = _mapper.Map<CourseDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.Course.CourseSearchQuery(_mapper, new CourseSearchRequest { Id = id });
                result = _mapper.Map<CourseDTO>(response.Data.SingleOrDefault());
            }

            return result;
        }

        public async Task<AdjustableDTO<CourseDTO>> GetAsync(bool useNoSql = true)
        {
            AdjustableDTO<CourseDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticCourse>(new CourseSearchRequest(), "course");
                result = new AdjustableDTO<CourseDTO>(response, _mapper.Map<List<CourseDTO>>(response.Data), response.Total);
            }
            else
            {
                result = await _context.Course.CourseSearchQuery(_mapper, new CourseSearchRequest());
            }

            return result;
        }


        public async Task<bool> SoftDeleteAsync(int id)
        {
            var course = await GetAsync(id);
            if (course != null)
            {
                course.IsActive = false;
                await CreateOrUpdateAsync(course);
            }
            else
            {
                Errors.Add(new Error("Course", "Unable to delete"));
                _logger.Error($"Course {id} deletion failed for user {UserId}");
            }

            return HasErrors;
        }
    }
}
