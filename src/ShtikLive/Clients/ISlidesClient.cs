using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ShtikLive.Clients
{
    public interface ISlidesClient
    {
        Task<string> Upload(string presenter, string show, int index, string contentType, byte[] source, CancellationToken ct = default);
        Task<HttpResponseMessage> Get(string presenter, string show, int index, CancellationToken ct = default);
    }
}