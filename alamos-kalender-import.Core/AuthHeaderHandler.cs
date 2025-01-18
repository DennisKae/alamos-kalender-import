using System;
using System.Collections.Generic;
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
        private readonly bool _disableLogging;

        public AuthHeaderHandler(ILogger logger, string apiToken) : base(new HttpClientHandler())
        {
            _logger = logger;
            _apiToken = apiToken;
            _disableLogging = Environment.GetEnvironmentVariable(SharedConstants.DisableApiLoggingEnvironmentVariableName)?.ToLower() == "true";
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

            if(!_disableLogging)
            {
                _logger.LogDebug($"Request: {request}");
                if(request.Content != null)
                {
                    string content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                    _logger.LogDebug($"Request Content: {content}");
                }
            }

            Stopwatch responseElapsedTime = Stopwatch.StartNew();
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if(!_disableLogging)
            {
                _logger.LogDebug($"Response: {response}");
                string content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogDebug($"Response Content: {content}");
            }

            responseElapsedTime.Stop();
            totalElapsedTime.Stop();

            if(!_disableLogging)
            {
                _logger.LogDebug($"Response elapsed time: {responseElapsedTime.ElapsedMilliseconds} ms");
                _logger.LogDebug($"Total elapsed time: {totalElapsedTime.ElapsedMilliseconds} ms");
            }

            return response;
        }
    }
}