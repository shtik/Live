using System.Threading;
using System.Threading.Tasks;
using ShtikLive.Models.Live;
using ShtikLive.Models.Present;

namespace ShtikLive.Clients
{
    public interface IShowsClient
    {
        Task<bool> ShowSlide(string presenter, string slug, int number, CancellationToken ct = default(CancellationToken));
        Task<Show> Get(string presenter, string slug, CancellationToken ct = default(CancellationToken));
        Task<Slide> GetSlide(string presenter, string slug, int number, CancellationToken ct = default(CancellationToken));
        Task<Show> Start(StartShow startShow, CancellationToken ct = default(CancellationToken));
        Task<Show> GetLatest(string presenter, CancellationToken ct = default(CancellationToken));
        Task<Slide> GetLatestSlide(string presenter, string slug, CancellationToken ct = default(CancellationToken));
    }
}