using System.Threading.Tasks;
using ShtikLive.Models.Live;

namespace ShtikLive.Clients
{
    public interface IShowsClient
    {
        Task<Show> Start(Show show);
        Task<Show> GetLatest(string presenter);
        Task<bool> ShowSlide(string presenter, string slug, int number);
        Task<Show> Get(string presenter, string slug);
        Task<Slide> GetSlide(string presenter, string slug, int number);
    }
}