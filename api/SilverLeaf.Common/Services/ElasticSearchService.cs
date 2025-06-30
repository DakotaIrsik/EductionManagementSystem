using AutoMapper;
using Elasticsearch.Net;
using Microsoft.Extensions.Options;
using Nest;
using SilverLeaf.Common.Helpers;
using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Services
{

    public interface IElasticSearchService : IDisposable
    {
        int Index<T>(T model, string index) where T : class;
        Task<int> IndexAsync<T>(T model, string index) where T : class;
        Task<AdjustableDTO<T>> Search<T>(object searchRequest, string index) where T : class;
        Task<int> IndexManyAsync<T>(IEnumerable<T> model, string index) where T : class;
        int IndexMany<T>(IEnumerable<T> model, string index) where T : class;
        Task<HttpResponseMessage> DeleteIndexAsync(string index);
        void CreateIndex(CreateIndexDescriptor descriptor);
        Task<HttpResponseMessage> CreateIndexAsync(string indexName, string mappingJson);
    }

    public class ElasticSearchService : IElasticSearchService, IDisposable
    {

        private readonly ElasticClient _client;
        private readonly IMapper _mapper;
        private readonly AppSettings _options;

        public ElasticSearchService(IOptions<AppSettings> options, IMapper mapper)
        {
            _mapper = mapper;
            _options = options.Value;
            _client = new ElasticClient(GetConfigSettings());
        }

        [Obsolete("Do not use this anymore, with ES 7.4 this is deprecated, instead you should manually create a mapping file.")]
        public void CreateIndex(CreateIndexDescriptor descriptor)
        {
            _client.CreateIndex(IndexName.From<CreateIndexDescriptor>(), d => descriptor);
        }

        public async Task<HttpResponseMessage> CreateIndexAsync(string indexName, string mappingJson)
        {
            var content = new StringContent(mappingJson, Encoding.UTF8, MediaTypeHeaderValue.Parse("application/json; charset=utf-8").MediaType);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.ConnectionStrings.ElasticSearch);
                return await client.PutAsync(indexName, content);
            }
        }

        public async Task<HttpResponseMessage> DeleteIndexAsync(string indexName)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.ConnectionStrings.ElasticSearch);
                return await client.DeleteAsync(indexName);
            }
        }

        public int Index<T>(T model, string index) where T : class
        {
            var value = model.GetType().GetProperties().SingleOrDefault(p => p.Name == "Id").GetValue(model);
            var result = _client.Index(model, m => m.Id(new Id(value)).Index(index));
            //var refreshIndex = _client.Refresh(index);
            return (!string.IsNullOrEmpty(result.Id)) ? 1 : 0;
        }

        public async Task<int> IndexAsync<T>(T model, string index) where T : class
        {
            var value = model.GetType().GetProperties().SingleOrDefault(p => p.Name == "Id").GetValue(model);
            var result = await _client.IndexAsync(model, m => m.Id(new Id(value)).Index(index));
            //var refreshIndex = await _client.RefreshAsync(index);
            return (!string.IsNullOrEmpty(result.Id)) ? 1 : 0;
        }

        public int IndexMany<T>(IEnumerable<T> model, string index) where T : class
        {
            var result = _client.IndexMany(model, index);
            //var refreshIndex = _client.Refresh(index);
            return result.Items.Count();
        }

        public async Task<int> IndexManyAsync<T>(IEnumerable<T> model, string index) where T : class
        {
            var result = await _client.IndexManyAsync(model, index);
            //var refreshIndex = await _client.RefreshAsync(index).ConfigureAwait(false);
            return result.Items.Count();
        }

        public async Task<AdjustableDTO<T>> Search<T>(object searchRequest, string index) where T : class
        {
            var args = ((IAdjustable)searchRequest);

            var result = await _client.SearchAsync<T>(q => q
                                                    .Sort(s2 => Sort<T>(args.Sort?.Split(',')))
                                                    .Query(y => Query(searchRequest))
                                                    .TrackScores()
                                                    .Source(s => new SourceFilter { Includes = args.Fields?.Replace(" ", "") })
                                                    .From(args.From)
                                                    .Index(index)
                                                    .Take(args.Size));

            var pagedResult = new AdjustableDTO<T>(args)
            {
                Total = result.Total,
                Data = _mapper.Map<List<T>>(result.Documents)
            };

            return pagedResult;
        }

        private QueryContainer Query<T>(T request)
        {
            QueryContainer container = new QueryContainer();

            var nonInheritedProperties = request.GetNonInheritedProperties();
            foreach (var property in nonInheritedProperties)
            {
                var value = property.GetValue(request);
                if (IsLookup(property, value))
                {
                    container &= (new MatchQuery
                    {
                        Field = property.Name,
                        Query = (value.ToString() == "True" || value.ToString() == "False") ? value.ToString().ToLower() : value.ToString()
                    });
                }
                else if (IsSearchable(property, value))
                {
                    container &= (new WildcardQuery
                    {
                        Field = property.Name,
                        Value = $"*{value.ToString().ToLower()}*"
                    });
                }
            }

            return container;
        }

        private SortDescriptor<IAdjustable> Sort<T>(IEnumerable<string> sortStrings)
        {
            var descriptor = new SortDescriptor<IAdjustable>();
            if (sortStrings?.Any() ?? false)
            {
                foreach (var sortString in sortStrings)
                {
                    if (typeof(T).GetProperty(SortablePropertyName(sortString)) != null)
                    {
                        descriptor.Field(SortablePropertyName(sortString), (sortString[0] == '-') ? SortOrder.Descending : SortOrder.Ascending);
                    }
                }
            }
            return descriptor;
        }

        private string SortablePropertyName(string sortString)
        {
            return sortString.Replace("-", "").Replace("+", "");
        }

        private bool IsSearchable(PropertyInfo property, object value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value.ToString() != "0";
        }

        private bool IsLookup(PropertyInfo property, object value)
        {
            var isLookup = false;
            //tired of fighting this property interogation battle, resorting to infinite if....
            switch (property.Name)
            {
                case "Id":
                    long.TryParse(value.ToString(), out var i);
                    isLookup = i > 0;
                    break;
                case "UserId":
                    isLookup = value != null && !string.IsNullOrWhiteSpace(value.ToString());
                    break;
                case "IsActive":
                    isLookup = true;
                    break;
                case "IsCorrect":
                    isLookup = true;
                    break;
                case "Fictionality":
                    isLookup = value != null && !string.IsNullOrWhiteSpace(value.ToString());
                    break;
                case "StudentId":
                    isLookup = value != null && !string.IsNullOrWhiteSpace(value.ToString());
                    long.TryParse(value.ToString(), out var l);
                    isLookup = l > 0;
                    break;
                case "HasCompletedScreener":
                    isLookup = value != null && !string.IsNullOrWhiteSpace(value.ToString());
                    isLookup = bool.TryParse(value.ToString(), out var m);
                    break;
                default:
                    isLookup = false;
                    break;
            }
            return isLookup;
        }

        private bool IsNumberGreaterThanZero(object value)
        {
            bool isNumber = long.TryParse(value.ToString(), out var i);
            return isNumber && i > 0;
        }

        private ConnectionSettings GetConfigSettings()
        {
            var urls = _options.ConnectionStrings.ElasticSearch.Split(',').Select(url => new Uri(url));
            var pool = new StaticConnectionPool(urls);
            var configSettings = new ConnectionSettings(pool)
                .DefaultFieldNameInferrer(fieldName => fieldName)
                .RequestTimeout(new TimeSpan(0, 0, 60));

            return configSettings;
        }

        public void Dispose()
        {
        }
    }
}
