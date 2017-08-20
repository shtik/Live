using System;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using ShtikLive.Models.Live;

namespace ShtikLive.Clients
{
    public class ShowsClient
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
    }
}