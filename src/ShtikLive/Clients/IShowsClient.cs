using System.Threading.Tasks;
using ShtikLive.Models.Live;
using ShtikLive.Models.Present;

namespace ShtikLive.Clients
{
    public interface IShowsClient
    {
        Task<Show> GetLatest(string presenter);
        Task<bool> ShowSlide(string presenter, string slug, int number);
        Task<Show> Get(string presenter, string slug);
        Task<Slide> GetSlide(string presenter, string slug, int number);
        Task<Show> Start(StartShow startShow);
    }
}