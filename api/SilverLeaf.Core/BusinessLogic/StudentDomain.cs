using AutoMapper;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Interfaces;
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
    public interface IStudentDomain
    {
        Task<StudentDTO> GetAsync(int id, bool useNoSql = true);
        Task<AdjustableDTO<StudentDTO>> GetAsync(PagingRequest paging, bool useNoSql = true);
        Task<AdjustableDTO<StudentDTO>> MyStudentsAsync(PagingRequest paging, bool useNoSql = true);
        Task<AdjustableDTO<StudentDTO>> SearchAsync(StudentSearchRequest request, bool useNoSql = true);
        Task<StudentDTO> CreateOrUpdateAsync(StudentDTO studio);
        Task<bool> SoftDeleteAsync(int id);
    }

    public class StudentDomain : BaseDomain, IStudentDomain
    {
        private readonly IImageService _imageService;
        private readonly IPhonicsScreenerDomain _phonicsScreener;
        private readonly IComprehensionScreenerDomain _comprehensionScreener;
        private readonly IOralScreenerDomain _oralScreener;

        public StudentDomain(EMSContext context,
            IPhonicsScreenerDomain phonicsScreener,
            IOralScreenerDomain oralScreener,
            IComprehensionScreenerDomain comprehensionScreener,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            IImageService imageService,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _phonicsScreener = phonicsScreener;
            _comprehensionScreener = comprehensionScreener;
            _oralScreener = oralScreener;
            _imageService = imageService;
        }

        public async Task<StudentDTO> CreateOrUpdateAsync(StudentDTO model)
        {
            var existingRecord = _context.Student.SingleOrDefault(s => s.Id == model.Id);
            var updatedRecord = _mapper.Map<Student>(model);
            if (existingRecord == null)
            {
                _context.Add(updatedRecord);
            }
            else
            { 
                _context.Entry(existingRecord).CurrentValues.SetValues(updatedRecord);
            }
            await _context.SaveChangesAsync();


            if (existingRecord == null)
            {
                await _phonicsScreener.Fill(updatedRecord.Id).ConfigureAwait(false);
                await _comprehensionScreener.Fill(updatedRecord.Id).ConfigureAwait(false);
                await _oralScreener.Fill(updatedRecord.Id).ConfigureAwait(false);
            }
            return _mapper.Map<StudentDTO>(updatedRecord);
        }

        public async Task<StudentDTO> GetAsync(int id, bool useNoSql = false)
        {
            StudentDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(new { Id = id }, "student");
                result = _mapper.Map<StudentDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.Student.FindAsync(id);
                result = _mapper.Map<StudentDTO>(response);
            }

            return result;
        }

        public async Task<AdjustableDTO<StudentDTO>> GetAsync(PagingRequest paging, bool useNoSql = true)
        {
            AdjustableDTO<StudentDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(new StudentSearchRequest(paging), "student");
                result = new AdjustableDTO<StudentDTO>(response, _mapper.Map<List<StudentDTO>>(response.Data), response.Total);
            }
            else
            {
                var response = await _context.Student.StudentSearchQuery(_mapper, new StudentSearchRequest(paging));
                result = _mapper.Map<AdjustableDTO<StudentDTO>>(response);
            }

            return result;
        }

        public async Task<AdjustableDTO<StudentDTO>> MyStudentsAsync(PagingRequest paging, bool useNoSql = true)
        {
            AdjustableDTO<StudentDTO> result = null;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(new StudentSearchRequest(paging), "reward");
                result = new AdjustableDTO<StudentDTO>(paging, _mapper.Map<List<StudentDTO>>(response.Data));
            }
            else
            {
                result = await _context.Student.StudentSearchQuery(_mapper, new StudentSearchRequest(paging));
            }

            return result;
        }

        public async Task<AdjustableDTO<StudentDTO>> SearchAsync(StudentSearchRequest request, bool useNoSql = true)
        {
            AdjustableDTO<StudentDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(request, "reward");
                result = new AdjustableDTO<StudentDTO>(response, _mapper.Map<List<StudentDTO>>(response.Data), response.Total);
            }
            else
            {
                result = await _context.Student.StudentSearchQuery(_mapper, request);
            }

            return result;
        }

        public async Task<AdjustableDTO<StudentDTO>> IncompleteScreenerStudentsAsync(IAdjustable pagingRequest, bool useNoSql = true)
        {
            AdjustableDTO<StudentDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(pagingRequest, "reward");
                result = new AdjustableDTO<StudentDTO>(response, _mapper.Map<List<StudentDTO>>(response.Data), response.Total);
            }
            else
            {
                result = await _context.PhonicsScreenerResult.IncompleteScreenerSearchQueryAsync(_mapper, pagingRequest);
            }

            return result;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var myRewards = await MyStudentsAsync(new PagingRequest());
            var studioDto = myRewards.Data.SingleOrDefault(d => d.Id == id);
            if (studioDto != null)
            {
                studioDto.IsActive = false;
                await CreateOrUpdateAsync(studioDto);
            }
            else
            {
                Errors.Add(new Error("Reward", "Unable to delete"));
                _logger.Error($"Reward {id} deletion failed for user {UserId}");
            }

            return HasErrors;
        }
    }
}
