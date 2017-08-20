using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using ShtikLive.Models.Live;

namespace ShtikLive.Clients
{
    public class ShowsClient : IShowsClient
    {
        private readonly HttpClient _http;

        public ShowsClient(IOptions<Options.Services> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Shows.BaseUrl)
            };
        }

        public async Task<Show> Start(Show show)
        {
            var response = await _http.PostJsonAsync("/shows/start", show).ConfigureAwait(false);
            return await response.Deserialize<Show>();
        }

        public async Task<Show> GetLatest(string presenter)
        {
            var response = await _http.GetAsync($"/shows/by/{presenter}/latest").ConfigureAwait(false);
            return await response.Deserialize<Show>();
        }

        public async Task<bool> ShowingSlide(int showId, int slideNumber)
        {
            var response = await _http.PutAsync($"slides/show/{showId}/{slideNumber}", new StringContent(string.Empty))
                .ConfigureAwait(false);
            if (response.IsSuccessStatusCode) return true;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            throw new UpstreamServiceException(response.StatusCode, response.ReasonPhrase);
        }
    }
}