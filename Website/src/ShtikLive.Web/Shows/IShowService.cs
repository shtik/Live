using System.Threading.Tasks;
using ShtikLive.Web.Models;

namespace ShtikLive.Web.Shows
{
    public interface IShowService
    {
        Task<Show> GetLatest(string presenter);
    }
}