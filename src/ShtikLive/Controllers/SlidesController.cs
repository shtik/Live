using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShtikLive.Clients;

namespace ShtikLive.Controllers
{
    [Route("slides")]
    public class SlidesController : Controller
    {
        private readonly ISlidesClient _client;

        public SlidesController(ISlidesClient client)
        {
            _client = client;
        }

        [HttpGet("{presenter}/{show}/{index}")]
        public async Task<IActionResult> Get(string presenter, string show, int index, CancellationToken ct)
        {
            var response = await _client.Get(presenter, show, index, ct);

            if (response.StatusCode != HttpStatusCode.OK) return new StatusCodeResult((int) response.StatusCode);

            return new StreamResult(await response.Content.ReadAsStreamAsync(), response.Content.Headers.ContentType.ToString());
        }
    }
}