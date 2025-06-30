using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.LookUps;
using SilverLeaf.Common.Models;
using SilverLeaf.Core.BusinessLogic;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ViewModels.Requests;
using SilverLeaf.Entities.ViewModels.Requests.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompletionScreenerController : BaseController
    {
        private ICompletionScreenerDomain _completionScreener { get; set; }
        public CompletionScreenerController(IBaseDomain baseDomain,
                                IOptions<AppSettings> configuration,
                                IHttpContextAccessor httpContext,
                                ILogger<CompletionScreenerController> logger,
                                IMapper mapper,
                                ICacheService cache,
                                ICompletionScreenerDomain completionScreener) : base(baseDomain, configuration, httpContext, logger, mapper, cache)
        {
            _completionScreener = completionScreener;
        }

        [HttpPost("Get")]
        [ProducesResponseType(typeof(List<ScreenerSummaryResponse>), 200)]
        public async Task<ActionResult<List<ScreenerSummaryResponse>>> Get(CompletedScreenerRequest model)
        {
            var response = await _completionScreener.GetAsync(model);
            var theList = new List<ScreenerSummaryResponse>();

            foreach (var item in response.Data)
            {
                var theRecord = response.Data.FirstOrDefault(r => r.Student.Id == item.Student.Id);
                List<List<PhonicsTask>> Tasks = new List<List<PhonicsTask>>();
                Tasks.Add(JsonConvert.DeserializeObject<List<PhonicsTask>>(item.Course1));
                Tasks.Add(JsonConvert.DeserializeObject<List<PhonicsTask>>(item.Course2));
                Tasks.Add(JsonConvert.DeserializeObject<List<PhonicsTask>>(item.Course3));
                Tasks.Add(JsonConvert.DeserializeObject<List<PhonicsTask>>(item.Course4));

                theList.Add(new ScreenerSummaryResponse()
                {
                    PhonicsMetrics = new List<PhonicsMetrics>()
                    {
                        new PhonicsMetrics()
                        {
                           PhonicsTasks = new List<PhonicsTask> {
                               Tasks[0].FirstOrDefault(),
                               Tasks[1].FirstOrDefault(),
                               Tasks[2].FirstOrDefault(),
                               Tasks[3].FirstOrDefault()
                           },
                        }
                    },
                    Student = theRecord.Student,

                    PrimaryRecommendation =theRecord.PrimaryRecommendation,
                    ReasonsForPrimaryRecommendation = theRecord.ReasonsForPrimaryRecommendation,

                    SecondaryRecommendation = theRecord.SecondaryRecommendation,
                    ReasonsForSecondaryRecommendation = theRecord.ReasonsForSecondaryRecommendation,
                    
                    AreasOfStrength = theRecord.AreasOfStrength,
                    AreasForImprovement = theRecord.AreasForImprovement,
                    Assessor = theRecord.Assessor,
                    ExtraInformationGained = theRecord.ExtraInformationGained,
                    GeneratedOn = theRecord.GeneratedOn,
                    LastScreenerDate = theRecord.LastScreenerDate,
                    StarReaderId = theRecord.StarReaderId,
                    Url = theRecord.Url
                });
            }

            var result = new AdjustableDTO<ScreenerSummaryResponse>(model, theList);

            return GetCustomResponse(result, model.Fields);
        }


        [HttpPost]
        [ProducesResponseType(typeof(CompletionScreenerDTO), 200)]
        public async Task<CompletionScreenerDTO> Submit(CompleteScreenerRequest model)
        {
            var request = Mapper.Map<CompletionScreenerDTO>(model);
            request.Student = new StudentDTO() { Id = model.StudentId };
            var result = await _completionScreener.SaveScreenerAsync(request);
            return result;
        }
    }
}