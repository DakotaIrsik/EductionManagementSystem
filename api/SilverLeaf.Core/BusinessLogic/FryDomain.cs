using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SilverLeaf.common.Services;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface IFryDomain
    {
        Task<AdjustableDTO<FryDTO>> Get(PagingRequest request);
    }

    public class FryDomain : BaseDomain, IFryDomain
    {
        public FryDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        { }


        public async Task<AdjustableDTO<FryDTO>> Get(PagingRequest request)
        {
            AdjustableDTO<FryDTO> result;
            var response = await _elastic.Search<ElasticFry>(request, "fry");
            result = new AdjustableDTO<FryDTO>(response, _mapper.Map<List<FryDTO>>(response.Data), response.Total);
            return result;
        }
    }
}
