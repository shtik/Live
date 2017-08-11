using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ShtikLive.Identity
{
    public class ApiKeyProvider : IApiKeyProvider
    {
        private readonly HMACSHA256 _hash;

        public ApiKeyProvider(IConfiguration configuration)
        {
            var hashString = configuration["Security:ApiKeyHashPhrase"];
            if (string.IsNullOrWhiteSpace(hashString))
            {
                throw new InvalidOperationException("Configuration error: 'Security:ApiKeyHashPhrase' not supplied.");
            }
            using (var sha = SHA512.Create())
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(hashString));
                _hash = new HMACSHA256(hashBytes);
            }
        }

        public string GetBase64(string userHandle)
        {
            var bytes = Encoding.UTF8.GetBytes(userHandle);
            var hash = _hash.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool CheckBase64(string userHandle, string hash)
        {
            return GetBase64(userHandle) == hash;
        }
    }
}