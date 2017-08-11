using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ShtikLive.Identity.Tests
{
    public class ApiKeyProviderTests
    {
        [Fact]
        public void GeneratesSensibleKey()
        {
            var config = new Dictionary<string, string>
            {
                ["Security:ApiKeyHashPhrase"] = "SecretSquirrel"
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
            var target = new ApiKeyProvider(configuration);
            var actual = target.GetBase64("steve");
            Assert.NotNull(actual);
            Assert.NotEqual(0, actual.Length);
        }

        [Fact]
        public void ValidatesHash()
        {
            var config = new Dictionary<string, string>
            {
                ["Security:ApiKeyHashPhrase"] = "SecretSquirrel"
            };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(config).Build();
            var target = new ApiKeyProvider(configuration);
            var hash = target.GetBase64("steve");

            Assert.True(target.CheckBase64("steve", hash));
        }
    }
}
