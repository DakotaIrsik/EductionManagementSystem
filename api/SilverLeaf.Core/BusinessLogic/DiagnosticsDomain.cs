using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.Common;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.Models;
using SilverLeaf.common.Services;

namespace SilverLeaf.Core.BusinessLogic
{

    public interface IDiagnosticsDomain
    {

    }
    public class DiagnosticsDomain : BaseDomain, IDiagnosticsDomain
    {
        public DiagnosticsDomain(EMSContext context, 
            IMapper mapper, 
            IElasticSearchService elastic, 
            ILogger logger, 
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
        }     
    }
}
