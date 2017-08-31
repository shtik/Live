using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ShtikLive.Models.Notes;
using ShtikLive.Models.Questions;

namespace ShtikLive.Clients
{
    public class QuestionsClient : IQuestionsClient
    {
        private readonly HttpClient _http;

        public QuestionsClient(IOptions<Options.Services> options)
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(options.Value.Questions.BaseUrl)
            };
        }

        public async Task<List<Question>> GetQuestionsForShow(string presenter, string slug, CancellationToken ct = default(CancellationToken))
        {
            var response = await _http.GetAsync($"{presenter}/{slug}", ct).ConfigureAwait(false);
            return await response.Deserialize<List<Question>>();
        }

        public async Task<List<Question>> GetQuestionsForSlide(string presenter, string slug, int number, CancellationToken ct = default(CancellationToken))
        {
            var response = await _http.GetAsync($"{presenter}/{slug}/{number}", ct)
                .ConfigureAwait(false);
            return await response.Deserialize<List<Question>>();
        }

        public async Task<Question> AskQuestion(string presenter, string slug, int slide, Question question,
            CancellationToken ct = default(CancellationToken))
        {
            var response = await _http.PostJsonAsync($"{presenter}/{slug}/{slide}", question, ct).ConfigureAwait(false);
            return await response.Deserialize<Question>();
        }
    }
}