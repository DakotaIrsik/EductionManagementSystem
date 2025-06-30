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
    public interface IClassDomain
    {
        Task<ClassDTO> CreateOrUpdateAsync(ClassDTO model);
        Task<ClassDTO> GetAsync(int id, bool useNoSql = true);
        Task<AdjustableDTO<ClassDTO>> SearchAsync(ClassSearchRequest request, bool useNoSql = true);
        Task<AdjustableDTO<ClassDTO>> GetAsync(bool useNoSql = true);
        Task<bool> SoftDeleteAsync(int id);
    }
    public class ClassDomain : BaseDomain, IClassDomain
    {
        public ClassDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
        }
        
        public async Task<ClassDTO> CreateOrUpdateAsync(ClassDTO model)
        {
            var course = _context.Class.SingleOrDefault(m => m.Id == model.Id);
            if (course == null)
            {
                _context.Class.Add(course);
            }
            else
            {
                course.IsActive = model.IsActive;
            }

            await ValidateAndSaveChangesAsync();
            return _mapper.Map<ClassDTO>(course);
        }

        public async Task<ClassDTO> GetAsync(int id, bool useNoSql = true)
        {
            ClassDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticClass>(new ClassSearchRequest { Id = id }, "class");
                result = _mapper.Map<ClassDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.Class.ClassSearchQuery(_mapper, new ClassSearchRequest { Id = id });
                result = _mapper.Map<ClassDTO>(response.Data.SingleOrDefault());
            }

            return result;
        }

        public async Task<AdjustableDTO<ClassDTO>> GetAsync(bool useNoSql = true)
        {
            AdjustableDTO<ClassDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticClass>(new ClassSearchRequest(new PagingRequest(true)), "class");
                result = new AdjustableDTO<ClassDTO>(response, _mapper.Map<List<ClassDTO>>(response.Data), response.Total);
            }
            else
            {
                result = await _context.Class.ClassSearchQuery(_mapper, new ClassSearchRequest(new PagingRequest(true)));
            }

            return result;
        }

        public async Task<AdjustableDTO<ClassDTO>> SearchAsync(ClassSearchRequest request, bool useNoSql = true)
        {
            AdjustableDTO<ClassDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticClass>(request, "class");
                result = new AdjustableDTO<ClassDTO>(response, _mapper.Map<List<ClassDTO>>(response.Data), response.Total);

            }
            else
            {
                result = await _context.Class.ClassSearchQuery(_mapper, request);
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
                Errors.Add(new Error("Class", "Unable to delete"));
                _logger.Error($"Class {id} deletion failed for user {UserId}");
            }

            return HasErrors;
        }
    }
}
