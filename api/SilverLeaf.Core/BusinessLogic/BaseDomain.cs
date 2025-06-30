using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using SilverLeaf.Common;
using SilverLeaf.Common.Logging;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SilverLeaf.common.Services;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface IBaseDomain
    {
        void ValidateAndSaveChanges();
        Task ValidateAndSaveChangesAsync();
        bool HasErrors { get; }
        bool HasMessages { get; }

        List<Error> GetErrors();

    }
    public class BaseDomain : IBaseDomain
    {
        protected readonly EMSContext _context;
        protected readonly IElasticSearchService _elastic;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IHttpContextAccessor _http;
        protected readonly IOptions<AppSettings> _settings;
        protected readonly ICacheService _cache;

        public List<Error> Errors { get; set; } = new List<Error>();
        public List<Message> Messages { get; set; } = new List<Message>();

        public BaseDomain(EMSContext context,
                            IMapper mapper,
                            IElasticSearchService elastic,
                            ILogger logger,
                            IHttpContextAccessor httpContext,
                            IOptions<AppSettings> settings,
                            ICacheService cache)
        {
            _context = context;
            _elastic = elastic;
            _mapper = mapper;
            _logger = logger;
            _http = httpContext;
            _settings = settings;
            _cache = cache;
        }


        public void ValidateAndSaveChanges()
        {
            try
            {
                var records = _context.SaveChanges();
                Messages.Add(new Message("DbRecords changed", records.ToString()));
            }
            catch (Exception ex)
            {
                var errorSummary = ex.ToFriendly();
                foreach (var item in errorSummary)
                {
                    Errors.Add(new Error(item.Key, item.Value));
                }
            }
        }

        public async Task ValidateAndSaveChangesAsync()
        {
            try
            {
                var records = await _context.SaveChangesAsync();
                Messages.Add(new Message("DbRecords changed", records.ToString()));
            }
            catch (Exception ex)
            {
                var errorSummary = ex.ToFriendly();
                foreach (var item in errorSummary)
                {
                    Errors.Add(new Error(item.Key, item.Value));
                }
            }
        }

        protected string UserId => _http?.HttpContext?.User?.Claims?.SingleOrDefault(c => c.Type == "sub")?.Value;

        public bool HasErrors => Errors.Any();

        public bool HasMessages => Messages.Any();

        public List<Error> GetErrors()
        {
            return Errors;
        }
    }
}
