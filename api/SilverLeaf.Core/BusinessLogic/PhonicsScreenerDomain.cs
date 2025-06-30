using AutoMapper;
using CacheExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Helpers;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface IPhonicsScreenerDomain
    {
        Task<PhonicsScreenerDTO> GetAsync(int? order, bool useNoSql = false);
        Task<List<PhonicsScreenerDTO>> GetAsync(bool useNoSql = false);
        Task<List<PhonicsScreenerDTO>> GetAsync(int courseId, int taskId, bool useNoSql = false);
        Task<PhonicsScreenerDTO> SubmitResponseAsync(PhonicsScreenerDTO model);
        Task<string> SubmitBulkResponseAsync(List<PhonicsScreenerDTO> models);
        Task<List<PhonicsScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false);
        Task<List<PhonicsScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest);
        Task<bool> Fill(int studentId);
    }
    public class PhonicsScreenerDomain : BaseDomain, IPhonicsScreenerDomain
    {
        public PhonicsScreenerDomain(EMSContext context,
                              IMapper mapper,
                              IElasticSearchService elastic,
                              ILogger logger,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AppSettings> settings,
                              ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {

            var response = _cache.GetOrSet<List<PhonicsScreenerDTO>>("PhonicsScreenerQuestions",
             _mapper.Map<List<PhonicsScreenerDTO>>(_context.PhonicsScreener.ToList())
             , new TimeSpan(24, 0, 0));

            _logger.Information($"{response.Count()} Phonics screener questions added to cache.");
        }

        public async Task<List<PhonicsScreenerDTO>> GetAsync(bool useNoSql = false)
        {
            List<PhonicsScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticPhonicsScreener>(new PhonicsScreenerSearchRequest(new PagingRequest { Size = 1000 }), "phonicsscreener");
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.PhonicsScreener.PhonicsScreenerSearchQuery(_mapper, new PhonicsScreenerSearchRequest());
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<PhonicsScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false)
        {
            List<PhonicsScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticPhonicsScreenerResult>(new PhonicsScreenerSearchRequest(pagingRequest) { StudentId = studentId }, "phonicsscreenerresult");
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.PhonicsScreenerResult.PhonicsScreenerResultSearchQuery(_mapper, new PhonicsScreenerSearchRequest(pagingRequest) { StudentId = studentId });
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<PhonicsScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest)
        {
            var response = await _context.PhonicsScreenerResult.PhonicsScreenerResultSearchQuery(_mapper, new PhonicsScreenerSearchRequest(pagingRequest) { StudentId = studentId, Incomplete = true, UpdatedBy = "System" });
            return _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
        }

        public async Task<PhonicsScreenerDTO> GetAsync(int? order, bool useNoSql = false)
        {
            PhonicsScreenerDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticPhonicsScreener>(new PhonicsScreenerSearchRequest { Order = order }, "phonicsscreener");
                result = _mapper.Map<PhonicsScreenerDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.PhonicsScreener.PhonicsScreenerSearchQuery(_mapper, new PhonicsScreenerSearchRequest { Order = order });
                result = _mapper.Map<PhonicsScreenerDTO>(response.Data.SingleOrDefault());
            }

            return result;
        }

        public async Task<List<PhonicsScreenerDTO>> GetAsync(int courseId, int task, bool useNoSql = false)
        {
            List<PhonicsScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticPhonicsScreener>(new PhonicsScreenerSearchRequest { CourseId = courseId, Task = task }, "phonicsscreener");
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.PhonicsScreener.PhonicsScreenerSearchQuery(_mapper, new PhonicsScreenerSearchRequest { CourseId = courseId, Task = task });
                result = _mapper.Map<List<PhonicsScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<bool> Fill(int studentId)
        {
            var screenerQuestions = _cache.GetOrSet<List<PhonicsScreenerDTO>>("PhonicsScreenerQuestions", 
                                                                            _mapper.Map<List<PhonicsScreenerDTO>>(_context.PhonicsScreener.ToList()), 
                                                                            new TimeSpan(24, 0, 0));
            var allScreenerQuestionsWithStudentIds = screenerQuestions.Select(asq => { asq.StudentId = studentId; return asq; }).ToList();
            var screenerResults = _mapper.Map<List<PhonicsScreenerResult>>(allScreenerQuestionsWithStudentIds);
            await _context.PhonicsScreenerResult.AddRangeAsync(screenerResults);
            var result = await _context.SaveChangesAsync();
            return result > 1;
        }

        public async Task<PhonicsScreenerDTO> SubmitResponseAsync(PhonicsScreenerDTO model)
        {
            var dbEntity = _context.PhonicsScreenerResult.Find(model.Id);
            dbEntity.IsCorrect = model.IsCorrect;
            _context.Update(dbEntity);
             await ValidateAndSaveChangesAsync();
            var x = Errors;
            return _mapper.Map<PhonicsScreenerDTO>(dbEntity);
        }

        public async Task<string> SubmitBulkResponseAsync(List<PhonicsScreenerDTO> models)
        {
            foreach (var item in models)
            {
                var dbEntity = _context.PhonicsScreenerResult.Find(item.Id);
                dbEntity.IsCorrect = item.IsCorrect;
                _context.Update(dbEntity);
            }
           
            await ValidateAndSaveChangesAsync();
            return Messages.FirstOrDefault().Messages.FirstOrDefault();
        }
    }
}
