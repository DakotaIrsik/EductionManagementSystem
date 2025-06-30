using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.Helpers;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface IPendingScreenerDomain
    {
        Task<AdjustableDTO<StudentDTO>> GetAsync(PendingScreenerSearchRequest pagingRequest);

    }
    public class PendingScreenerDomain : BaseDomain, IPendingScreenerDomain
    {
        private readonly IPhonicsScreenerDomain _phonicsScreener;
        private readonly IOralScreenerDomain _oralScreener;
        public PendingScreenerDomain(EMSContext context,
                              IMapper mapper,
                              IElasticSearchService elastic,
                              ILogger logger,
                              IHttpContextAccessor httpContextAccessor,
                              IOptions<AppSettings> settings,
                              ICacheService cache,
                              IPhonicsScreenerDomain phonicsScreenerDomain,
                              IOralScreenerDomain oralScreenerDomain) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _phonicsScreener = phonicsScreenerDomain;
            _oralScreener = oralScreenerDomain;
        }

        public async Task<AdjustableDTO<StudentDTO>> GetAsync(PendingScreenerSearchRequest request)
        {
            var result = await _context.Student.PendingScreenerSearchQuery(_mapper, request);
            var totalPhonics = _context.PhonicsScreener.Count();
            var totalOral = _context.OralScreener.Count();
            var totalComprehension = _context.ComprehensionScreener.Count();

            var data = result.Data.ToList();
            for (int i = 0; i < data.Count(); i++)
            {
                var student = result.Data.FirstOrDefault(s => s.Id == data[i].Id);
                var completedPhonics = _context.PhonicsScreenerResult.Where(psr => psr.StudentId == data[i].Id && psr.IsCorrect != null).Count();
                var completedOral = _context.OralScreenerResult.Where(psr => psr.StudentId == data[i].Id && psr.IsCorrect != null).Count();
                var completedComprehension = _context.ComprehensionScreenerResult.Where(psr => psr.StudentId == data[i].Id && psr.IsCorrect != null).Count();

                student.PhonicsScreenerCompletionPercentage = (int)Math.Floor(((double)completedPhonics / (double)totalPhonics) * 100);
                student.OralScreenerCompletionPercentage = (int)Math.Floor(((double)completedOral / (double)totalOral) * 100);
                student.ComprehensionScreenerCompletionPercentage = (int)Math.Floor(((double)completedComprehension / (double)totalComprehension) * 100);

            }
            return result;
        }
    }
}
