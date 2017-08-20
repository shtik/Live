using System.Threading.Tasks;
using ShtikLive.Models.Live;

namespace ShtikLive.Web.Shows
{
    public interface IShowService
    {
        Task<Show> GetLatest(string presenter);
    }
}