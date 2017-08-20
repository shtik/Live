using System.Threading.Tasks;
using ShtikLive.Models.Live;

namespace ShtikLive.Clients
{
    public interface IShowsClient
    {
        Task<Show> Start(Show show);
        Task<Show> GetLatest(string presenter);
        Task<bool> ShowingSlide(int showId, int slideNumber);
    }
}