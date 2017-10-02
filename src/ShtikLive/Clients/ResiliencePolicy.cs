using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Wrap;

namespace ShtikLive.Clients
{
    public static class ResiliencePolicy
    {
        public static Policy<T> Create<T>(ILogger logger, Func<T, bool> resultPredicate, Func<CancellationToken, Task<T>> fallbackFunc = null)
        {
            fallbackFunc = fallbackFunc ?? (_ => Task.FromResult(default(T)));

            var retry = Policy.Handle<Exception>(e => !(e is BrokenCircuitException))
                .OrResult(resultPredicate)
                .WaitAndRetryForeverAsync(n => TimeSpan.FromMilliseconds(Math.Min((n + 1) * 100, 500)),
                    (result, _) =>
                    {
                        if (result.Exception != null)
                        logger.LogWarning(result.Exception, result.Exception.Message);
                    });

            var breaker = Policy.Handle<Exception>()
                .OrResult(resultPredicate)
                .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1),
                (result, _) => logger.LogWarning("Circuit breaker tripped.") ,
                () => logger.LogWarning("Circuit breaker reset."));

            var fallback = Policy<T>.Handle<Exception>().FallbackAsync(fallbackFunc);

            var retryAndBreak = Policy.WrapAsync(retry, breaker);

            return fallback.WrapAsync(retryAndBreak);
        }

        public static Policy<HttpResponseMessage> CreateHttp(ILogger logger)
        {
            return Create<HttpResponseMessage>(logger, r => (int) r.StatusCode >= 500);
        }
    }
}