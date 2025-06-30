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
    public interface IPhonicsSkillDomain
    {
        Task<AdjustableDTO<PhonicsSkillDTO>> Get(PagingRequest request);
    }

    public class PhonicsSkillDomain : BaseDomain, IPhonicsSkillDomain
    {
        public PhonicsSkillDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        { }


        public async Task<AdjustableDTO<PhonicsSkillDTO>> Get(PagingRequest request)
        {
            AdjustableDTO<PhonicsSkillDTO> result;
            var response = await _elastic.Search<ElasticPhonicsSkill>(request, "phonicsskill");
            result = new AdjustableDTO<PhonicsSkillDTO>(response, _mapper.Map<List<PhonicsSkillDTO>>(response.Data), response.Total);
            return result;
        }
    }
}
