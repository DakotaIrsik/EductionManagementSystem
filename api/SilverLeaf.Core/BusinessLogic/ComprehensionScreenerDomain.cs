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
    public interface IComprehensionScreenerDomain
    {
        Task<ComprehensionScreenerDTO> GetAsync(int? order, bool useNoSql = false);
        Task<List<ComprehensionScreenerDTO>> GetAsync(bool useNoSql = false);
        Task<List<ComprehensionScreenerDTO>> GetAsync(string course, string task, bool useNoSql = false);
        Task<ComprehensionScreenerDTO> SubmitResponseAsync(ComprehensionScreenerDTO model);
        Task<List<ComprehensionScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false);
        Task<List<ComprehensionScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest);
        Task<bool> Fill(int studentId);
        Task<string> SubmitBulkResponseAsync(List<ComprehensionScreenerDTO> models);
    }
    public class ComprehensionScreenerDomain : BaseDomain, IComprehensionScreenerDomain
    {
        public ComprehensionScreenerDomain(EMSContext context,
                              IMapper mapper,
                              IElasticSearchService elastic,
                              ILogger logger,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AppSettings> settings,
                              ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            var response = _cache.GetOrSet<List<ComprehensionScreenerDTO>>("ComprehensionScreenerQuestions",
               _mapper.Map<List<ComprehensionScreenerDTO>>(_context.ComprehensionScreener.ToList())
               , new TimeSpan(24, 0, 0));

            _logger.Information($"{response.Count()} Comprehension screener questions added to cache.");

        }

        public async Task<List<ComprehensionScreenerDTO>> GetAsync(bool useNoSql = false)
        {
            List<ComprehensionScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticComprehensionScreener>(new ComprehensionScreenerSearchRequest(new PagingRequest { Size = 1000 }), "comprehensionscreener");
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.ComprehensionScreener.ComprehensionScreenerSearchQuery(_mapper, new ComprehensionScreenerSearchRequest());
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<ComprehensionScreenerDTO>> GetByStudentAsync(int studentId, PagingRequest pagingRequest, bool useNoSql = false)
        {
            List<ComprehensionScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticComprehensionScreenerResult>(new ComprehensionScreenerSearchRequest(pagingRequest) { StudentId = studentId }, "comprehensionscreenerresult");
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.ComprehensionScreenerResult.ComprehensionScreenerResultSearchQuery(_mapper, new ComprehensionScreenerSearchRequest(pagingRequest) { StudentId = studentId });
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }

            return result;
        }

        public async Task<List<ComprehensionScreenerDTO>> GetIncompleteByStudentAsync(int studentId, PagingRequest pagingRequest)
        {
            var response = await _context.ComprehensionScreenerResult.ComprehensionScreenerResultSearchQuery(_mapper, new ComprehensionScreenerSearchRequest(pagingRequest) { StudentId = studentId, Incomplete = true });
            return _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
        }

        public async Task<ComprehensionScreenerDTO> GetAsync(int? order, bool useNoSql = false)
        {
            ComprehensionScreenerDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticComprehensionScreener>(new ComprehensionScreenerSearchRequest { Order = order }, "comprehensionscreener");
                result = _mapper.Map<ComprehensionScreenerDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.ComprehensionScreener.ComprehensionScreenerSearchQuery(_mapper, new ComprehensionScreenerSearchRequest { Order = order });
                result = _mapper.Map<ComprehensionScreenerDTO>(response.Data.SingleOrDefault());
            }

            return result;
        }

        public async Task<List<ComprehensionScreenerDTO>> GetAsync(string question, string image, bool useNoSql = false)
        {
            List<ComprehensionScreenerDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticComprehensionScreener>(new ComprehensionScreenerSearchRequest { Question = question, Image = image }, "comprehensionscreener");
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }
            else
            {
                var response = await _context.ComprehensionScreener.ComprehensionScreenerSearchQuery(_mapper, new ComprehensionScreenerSearchRequest { Question = question, Image = image });
                result = _mapper.Map<List<ComprehensionScreenerDTO>>(response.Data);
            }

            return result;
        }


        public async Task<bool> Fill(int studentId)
        {
            var screenerQuestions = _cache.GetOrSet<List<ComprehensionScreenerDTO>>("ComprehensionScreenerQuestions",
                                                                            _mapper.Map<List<ComprehensionScreenerDTO>>(_context.ComprehensionScreener.ToList()), 
                                                                            new TimeSpan(24, 0, 0));
            var allScreenerQuestionsWithStudentIds = screenerQuestions.Select(asq => { asq.StudentId = studentId; return asq; }).ToList();
            var screenerResults = _mapper.Map<List<ComprehensionScreenerResult>>(allScreenerQuestionsWithStudentIds);
            await _context.ComprehensionScreenerResult.AddRangeAsync(screenerResults);
            var result = await _context.SaveChangesAsync();
            return result > 1;
        }

        public async Task<ComprehensionScreenerDTO> SubmitResponseAsync(ComprehensionScreenerDTO model)
        {
            var dbEntity = _mapper.Map<ComprehensionScreenerResult>(model);
            dbEntity.Id = model.Id;
            _context.Update(dbEntity);
            await ValidateAndSaveChangesAsync();
            return _mapper.Map<ComprehensionScreenerDTO>(dbEntity);
        }

        public async Task<string> SubmitBulkResponseAsync(List<ComprehensionScreenerDTO> models)
        {
            foreach (var item in models)
            {
                var dbEntity = _context.ComprehensionScreenerResult.Find(item.Id);
                dbEntity.IsCorrect = item.IsCorrect;
                _context.Update(dbEntity);
            }

            await ValidateAndSaveChangesAsync();
            return Messages.FirstOrDefault().Messages.FirstOrDefault();
        }
    }
}
