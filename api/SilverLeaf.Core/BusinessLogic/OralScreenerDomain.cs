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
    public interface IOralScreenerDomain
    {
        Task<OralScreenerDTO> GetAsync(int? order, bool useNoSql = false);
        Task<List<OralScreenerDTO>> GetAsync(bool useNoSql = false);
        Task<List<OralScreenerDTO>> GetAsync(string course, string task, bool useNoSql = false);
        Task<OralScreenerDTO> SubmitResponseAsync(OralScreenerDTO model);
        Task<List<OralScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false);
        Task<List<OralScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest);
        Task<bool> Fill(int studentId);
        Task<string> SubmitBulkResponseAsync(List<OralScreenerDTO> models);
    }
    public class OralScreenerDomain : BaseDomain, IOralScreenerDomain
    {
        public OralScreenerDomain(EMSContext context,
                              IMapper mapper,
                              IElasticSearchService elastic,
                              ILogger logger,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AppSettings> settings,
                              ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            var response = _cache.GetOrSet<List<OralScreenerDTO>>("OralScreenerQuestions",
                _mapper.Map<List<OralScreenerDTO>>(_context.OralScreener.ToList())
                , new TimeSpan(24, 0, 0));

            _logger.Information($"{response.Count()} Oral screener questions added to cache.");
        }

        public async Task<List<OralScreenerDTO>> GetAsync(bool useNoSql = false)
        {
            List<OralScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticOralScreener>(new OralScreenerSearchRequest(new PagingRequest { Size = 1000 }), "oralscreener");
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.OralScreener.OralScreenerSearchQuery(_mapper, new OralScreenerSearchRequest());
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<OralScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false)
        {
            List<OralScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticOralScreenerResult>(new OralScreenerSearchRequest(pagingRequest) { StudentId = studentId }, "oralscreenerresult");
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.OralScreenerResult.OralScreenerResultSearchQuery(_mapper, new OralScreenerSearchRequest(pagingRequest) { StudentId = studentId });
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<OralScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest)
        {
            var response = await _context.OralScreenerResult.OralScreenerResultSearchQuery(_mapper, new OralScreenerSearchRequest(pagingRequest) { StudentId = studentId, Incomplete = true, UpdatedBy = "System" });
            return _mapper.Map<List<OralScreenerDTO>>(response.Data);
        }

        public async Task<OralScreenerDTO> GetAsync(int? order, bool useNoSql = false)
        {
            OralScreenerDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticOralScreener>(new OralScreenerSearchRequest { Order = order }, "oralscreener");
                result = _mapper.Map<OralScreenerDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.OralScreener.OralScreenerSearchQuery(_mapper, new OralScreenerSearchRequest { Order = order });
                result = _mapper.Map<OralScreenerDTO>(response.Data.SingleOrDefault());
            }

            return result;
        }

        public async Task<List<OralScreenerDTO>> GetAsync(string question, string image, bool useNoSql = false)
        {
            List<OralScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticOralScreener>(new OralScreenerSearchRequest { Question = question, Image = image }, "oralscreener");
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.OralScreener.OralScreenerSearchQuery(_mapper, new OralScreenerSearchRequest { Question = question, Image = image });
                result = _mapper.Map<List<OralScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<bool> Fill(int studentId)
        {
            var screenerQuestions = _cache.GetOrSet<List<OralScreenerDTO>>("OralScreenerQuestions",
                                                                            _mapper.Map<List<OralScreenerDTO>>(_context.OralScreener.ToList()), 
                                                                            new TimeSpan(24, 0, 0));
            var allScreenerQuestionsWithStudentIds = screenerQuestions.Select(asq => { asq.StudentId = studentId; return asq; }).ToList();
            var screenerResults =_mapper.Map<List<OralScreenerResult>>(allScreenerQuestionsWithStudentIds);
            await _context.OralScreenerResult.AddRangeAsync(screenerResults);
            var result = await _context.SaveChangesAsync();
            return result > 1;
        }

        public async Task<OralScreenerDTO> SubmitResponseAsync(OralScreenerDTO model)
        {
            var dbEntity = _mapper.Map<OralScreenerResult>(model);
            dbEntity.Id = model.Id;
            _context.Update(dbEntity);
            await ValidateAndSaveChangesAsync();
            return _mapper.Map<OralScreenerDTO>(dbEntity);
        }

        public async Task<string> SubmitBulkResponseAsync(List<OralScreenerDTO> models)
        {
            foreach (var item in models)
            {
                var dbEntity = _context.OralScreenerResult.Find(item.Id);
                dbEntity.IsCorrect = item.IsCorrect;
                _context.Update(dbEntity);
            }

            await ValidateAndSaveChangesAsync();
            return Messages.FirstOrDefault().Messages.FirstOrDefault();
        }
    }
}
