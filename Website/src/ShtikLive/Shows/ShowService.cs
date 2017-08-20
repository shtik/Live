using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShtikLive.Models.Live;

namespace ShtikLive.Web.Shows
{
    public class ShowService : IShowService
    {
        private readonly HttpClient _http;
        public ShowService(IOptions<ShowOptions> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.ServiceUrl)
            };
        }

        public async Task<Show> GetLatest(string presenter)
        {
            var response = await _http.GetAsync($"/latest/{presenter}");
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Show>(await response.Content.ReadAsStringAsync());
            }
            return null;
        }
    }
}