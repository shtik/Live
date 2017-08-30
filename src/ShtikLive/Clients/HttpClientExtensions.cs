using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShtikLive.Clients
{
    public static class HttpClientExtensions
    {
        private const string ApplicationJson = "application/json";

        public static Task<HttpResponseMessage> PostJsonAsync(this HttpClient client, string uri, object obj, CancellationToken ct = default(CancellationToken))
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            return client.PostAsync(uri, content, ct);
        }

        public static Task<HttpResponseMessage> PutJsonAsync(this HttpClient client, string uri, object obj, CancellationToken ct = default(CancellationToken))
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, ApplicationJson);
            return client.PutAsync(uri, content, ct);
        }

        public static ValueTask<T> Deserialize<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ValueTask<T>(default(T));
                }
                throw new UpstreamServiceException(response.StatusCode, response.ReasonPhrase);
            }

            async Task<T> DeserializeAsync()
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(content);
            }

            return new ValueTask<T>(DeserializeAsync());
        }
    }
}