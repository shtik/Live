using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Shtik.Render.Markdown.Tests
{
    public class SplitterTests
    {
        [Fact]
        public async Task IgnoresLeadingTripleDash()
        {
            var text = "---\nPass\n---";
            using (var target = new Splitter(CreateStream(text)))
            {
                var actual = await target.ReadNextBlockAsync();
                Assert.Equal("Pass", actual);
            }
        }

        [Fact]
        public async Task SplitsAtTripleDash()
        {
            var text = "---\nOne\n---\n---\nTwo\n---";
            using (var target = new Splitter(CreateStream(text)))
            {
                var one = await target.ReadNextBlockAsync();
                var two = await target.ReadNextBlockAsync();
                Assert.Equal("One", one);
                Assert.Equal("Two", two);
            }
        }

        [Fact]
        public async Task DoesNotNeedTripleDashAtEnd()
        {
            var text = "---\nOne\n---\n---\nTwo";
            using (var target = new Splitter(CreateStream(text)))
            {
                var one = await target.ReadNextBlockAsync();
                var two = await target.ReadNextBlockAsync();
                Assert.Equal("One", one);
                Assert.Equal("Two", two);
            }
        }

        private static MemoryStream CreateStream(string text)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(text));
        }
    }
}
