using Xunit;

namespace Shtik.Render.Markdown.Tests
{
    public class RendererTests
    {
        [Fact]
        public void RendersYaml()
        {
            const string frontMatter = "title: Pass";
            const string markdown = "# Pass";
            var target = new Renderer();
            var actual = target.Render(frontMatter, markdown);
            Assert.Equal("Pass", actual.Metadata["title"]);
        }
    }
}
