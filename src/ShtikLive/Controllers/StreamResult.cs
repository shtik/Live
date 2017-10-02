using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShtikLive.Controllers
{
    public sealed class StreamResult : IActionResult, IDisposable
    {
        private readonly string _contentType;
        private readonly Stream _stream;

        public StreamResult(Stream stream, string contentType)
        {
            _contentType = contentType;
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 200;
            if (!string.IsNullOrWhiteSpace(_contentType))
            {
                response.ContentType = _contentType;
            }
            if (_stream.Length > 0)
            {
                response.ContentLength = _stream.Length;
            }
            await _stream.CopyToAsync(response.Body);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}