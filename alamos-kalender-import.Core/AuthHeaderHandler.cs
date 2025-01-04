using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DennisKae.alamos_kalender_import.Core
{
    [DebuggerStepThrough]
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILogger _logger;
        private readonly string _apiToken;
        
        public AuthHeaderHandler(ILogger logger, string apiToken) : base(new HttpClientHandler())
        {
            _logger = logger;
            _apiToken = apiToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            var totalElapsedTime = Stopwatch.StartNew();

            if(!string.IsNullOrWhiteSpace(_apiToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("JWT", _apiToken);
            }
            
            _logger.LogDebug($"Request: {request}");
            if (request.Content != null)
            {
                string content = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogDebug($"Request Content: {content}");
            }

            var responseElapsedTime = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);

            _logger.LogDebug($"Response: {response}");
            if (response.Content != null)
            {
                string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug($"Response Content: {content}");
            }

            responseElapsedTime.Stop();
            _logger.LogDebug($"Response elapsed time: {responseElapsedTime.ElapsedMilliseconds} ms");

            totalElapsedTime.Stop();
            _logger.LogDebug($"Total elapsed time: {totalElapsedTime.ElapsedMilliseconds} ms");

            return response;
        }
    }
}