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
    public interface IFeedbackDomain
    {
        FeedbackDTO CreateOrUpdate(FeedbackDTO model);
        Task<AdjustableDTO<FeedbackDTO>> Get(PagingRequest request);
    }

    public class FeedbackDomain : BaseDomain, IFeedbackDomain
    {
        public FeedbackDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        { }

        public FeedbackDTO CreateOrUpdate(FeedbackDTO model)
        {
            var tfsLink = $@"<a target='_blank' href='https://shorelinegames.com/tfs/RewardGrabber/_workitems/edit/" + model.WorkItem + "'>" + model.WorkItem + "</a>";
            model.WorkItem = tfsLink;
            var feedback = _mapper.Map<Feedback>(model);
            _context.Add(feedback);
            _context.SaveChanges();
            return _mapper.Map<FeedbackDTO>(feedback);
        }


        public async Task<AdjustableDTO<FeedbackDTO>> Get(PagingRequest request)
        {
            AdjustableDTO<FeedbackDTO> result;
            var response = await _elastic.Search<ElasticFeedback>(request, "feedback");
            result = new AdjustableDTO<FeedbackDTO>(response, _mapper.Map<List<FeedbackDTO>>(response.Data), response.Total);
            return result;
        }
    }
}
