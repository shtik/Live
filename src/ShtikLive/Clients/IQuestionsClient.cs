using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShtikLive.Models.Questions;

namespace ShtikLive.Clients
{
    public interface IQuestionsClient
    {
        Task<List<Question>> GetQuestionsForShow(string presenter, string slug, CancellationToken ct = default(CancellationToken));
        Task<List<Question>> GetQuestionsForSlide(string presenter, string slug, int number, CancellationToken ct = default(CancellationToken));

        Task<Question> AskQuestion(string presenter, string slug, int slide, Question question,
            CancellationToken ct = default(CancellationToken));
    }
}