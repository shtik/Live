using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using ShtikLive.Options;

namespace ShtikLive.Clients
{
    public class SlidesClient : ISlidesClient
    {
        private readonly HttpClient _http;
        private readonly Policy<HttpResponseMessage> _uploadPolicy;
        private readonly Policy<HttpResponseMessage> _getPolicy;

        public SlidesClient(IOptions<ServiceOptions> options, ILogger<SlidesClient> logger)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Slides.BaseUrl)
            };

            _uploadPolicy = ResiliencePolicy.CreateHttp(logger);
            _getPolicy = ResiliencePolicy.CreateHttp(logger);
        }

        public async Task<string> Upload(string presenter, string show, int index, string contentType, byte[] source,
            CancellationToken ct = default)
        {
            var content = new ByteArrayContent(source);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

            var uri = $"{presenter}/{show}/{index}";
            var response = await _uploadPolicy.ExecuteAsync(() => _http.PutAsync(uri, content, ct));
            return response == null ? null : uri;
        }

        public Task<HttpResponseMessage> Get(string presenter, string show, int index, CancellationToken ct = default)
        {
            return _getPolicy.ExecuteAsync(() => _http.GetAsync($"{presenter}/{show}/{index}", ct));
        }
    }
}