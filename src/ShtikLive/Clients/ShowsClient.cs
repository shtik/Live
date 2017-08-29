using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shtik.Rendering.Markdown;
using ShtikLive.Models.Present;
using Show = ShtikLive.Models.Live.Show;
using Slide = ShtikLive.Models.Live.Slide;

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

        public async Task<Show> Start(StartShow startShow)
        {
            var showRenderer = new ShowRenderer();
            var renderedShow = showRenderer.Render(startShow.Markdown);
            var show = new Show
            {
                Place = startShow.Place,
                Presenter = startShow.Presenter,
                Slug = startShow.Slug,
                Time = startShow.Time,
                Title = renderedShow.Metadata.GetStringOrDefault("title", startShow.Title),
                Slides = renderedShow.Slides.Select((s,i) => new Slide
                {
                    Layout = (s.Metadata, renderedShow.Metadata).GetStringOrDefault("layout", "title-and-content"),
                    Html = s.Html,
                    Title = s.Metadata.GetStringOrDefault("title", string.Empty),
                    Slug = startShow.Slug,
                    Presenter = startShow.Presenter,
                    Number = i
                }).ToList()
            };
            var response = await _http.PostJsonAsync("/shows/start", show).ConfigureAwait(false);
            return await response.Deserialize<Show>();
        }

        public async Task<Show> GetLatest(string presenter)
        {
            var response = await _http.GetAsync($"/shows/find/by/{presenter}/latest").ConfigureAwait(false);
            return await response.Deserialize<Show>();
        }

        public async Task<Show> Get(string presenter, string slug)
        {
            var response = await _http.GetAsync($"/shows/{presenter}/{slug}").ConfigureAwait(false);
            return await response.Deserialize<Show>();
        }

        public async Task<Slide> GetSlide(string presenter, string slug, int number)
        {
            var response = await _http.GetAsync($"/slides/{presenter}/{slug}/{number}").ConfigureAwait(false);
            return await response.Deserialize<Slide>();
        }

        public async Task<bool> ShowSlide(string presenter, string slug, int number)
        {
            var response = await _http.PutAsync($"slides/show/{presenter}/{slug}/{number}", new StringContent(string.Empty))
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