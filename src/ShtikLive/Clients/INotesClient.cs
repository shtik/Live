using System.Threading;
using System.Threading.Tasks;
using ShtikLive.Models.Notes;

namespace ShtikLive.Clients
{
    public interface INotesClient
    {
        Task<Note> GetNotes(string user, string presenter, string slug, int number, CancellationToken ct = default(CancellationToken));
        Task<bool> SaveNotes(string user, string presenter, string slug, int number, Note note, CancellationToken ct = default(CancellationToken));
    }
}