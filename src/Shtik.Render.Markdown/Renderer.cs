using SharpYaml.Serialization;
using Markdig;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace Shtik.Render.Markdown
{
    public class Renderer
    {
        private readonly Serializer _serializer = new Serializer();

        public Slide Render(string frontMatter, string markdown)
        {
            var metadata = new Dictionary<string, object>();
            _serializer.DeserializeInto(frontMatter, metadata);
            return new Slide
            {
                Metadata = metadata,
                Html = Markdig.Markdown.ToHtml(markdown)
            };
        }
    }
}