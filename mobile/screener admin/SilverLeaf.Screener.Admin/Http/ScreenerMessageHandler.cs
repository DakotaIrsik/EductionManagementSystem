//using ModernHttpClient;

namespace SilverLeaf.Screener.Admin.Http
{
    //    public class ScreenerMessageHandler : NativeMessageHandler
    //    {
    //        private readonly Config.Api.Header[] _headers;

    //        public ScreenerMessageHandler(Config.Api.Header[] headers)
    //        {
    //            _headers = headers;
    //        }

    //        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
    //            CancellationToken cancellationToken)
    //        {
    //#if DEBUG
    //            System.Diagnostics.Debug.WriteLine(request.RequestUri);
    //#endif

    //            // Add headers.
    //            //if (_headers != null)
    //            //{
    //            //    foreach (var header in _headers)
    //            //    {
    //            //        request.Headers.Add(header.Key, header.Value);
    //            //    }
    //            //}

    //            //var token = CacheService.Token;
    //            //if (!string.IsNullOrWhiteSpace(token))
    //            //{
    //            //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //            //}

    //            var httpResponseMessage = new HttpResponseMessage();

    //            // If an HttpRequestException occurs, retry the call up to 3 times.  If an exception is still thrown
    //            // then display error message to user
    //            //var retryPolicy = Policy
    //            //    .Handle<HttpRequestException>()
    //            //    .Or<WebException>()
    //            //    .WaitAndRetryAsync(3, waitBetweenAttempts => TimeSpan.FromSeconds(Math.Pow(2, waitBetweenAttempts)));

    //            //var policy = Policy
    //            //    .Handle<Exception>()
    //            //    .FallbackAsync(ct => throw new AppError("Uh oh! Something went wrong. Please try again."));


    //                httpResponseMessage = await base.SendAsync(request, cancellationToken);


    //            if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
    //            {
    //                CacheService.User = null;
    //                throw new AppError();
    //            }

    //            if (httpResponseMessage.Content != null)
    //            {
    //                var body = await httpResponseMessage.Content.ReadAsStringAsync();
    //                if (body != null)
    //                {
    //                    try
    //                    {
    //                        var message = JsonConvert.DeserializeObject<MessageError>(body);
    //                        if (message?.Message != null)
    //                        {
    //                            throw new AppError(message.Message);
    //                        }

    //                        var response = JsonConvert.DeserializeObject<Response>(body);
    //                        if (response != null && response.Errors?.Count > 0)//!response.Succeed)
    //                        {
    //                            throw new AppError(response.Errors);
    //                        }
    //                    }
    //                    catch (JsonException)
    //                    {
    //                        // ignore
    //                    }
    //                }
    //            }

    //            if (!httpResponseMessage.IsSuccessStatusCode)
    //            {
    //                throw new AppError(httpResponseMessage.ReasonPhrase);
    //            }

    //            return httpResponseMessage;
    //        }
    //    }
}
