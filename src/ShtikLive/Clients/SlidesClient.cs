using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ShtikLive.Options;

namespace ShtikLive.Clients
{
    public class SlidesClient : ISlidesClient
    {
        private readonly HttpClient _http;

        public SlidesClient(IOptions<ServiceOptions> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Slides.BaseUrl)
            };
        }

        public async Task<string> Upload(string presenter, string show, int index, string contentType, Stream source,
            CancellationToken ct = default)
        {
            var content = new StreamContent(source);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

            var uri = $"{presenter}/{show}/{index}";
            await _http.PostAsync(uri, content, ct);
            return uri;
        }

        public Task<HttpResponseMessage> Get(string presenter, string show, int index, CancellationToken ct = default)
        {
            return _http.GetAsync($"{presenter}/{show}/{index}", ct);
        }
    }
}