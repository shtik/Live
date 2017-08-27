using System;
using System.Collections.Generic;
using System.Diagnostics;
using Markdig;
using SharpYaml.Serialization;

namespace ShtikLive.Rendering.Markdown
{
    public class Renderer
    {
        private readonly Serializer _serializer = new Serializer();
        private readonly MarkdownPipeline _pipeline;

        public Renderer()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder.Use<HtmlSanitizerExtension>();
            _pipeline = pipelineBuilder.Build();
        }

        public Slide Render(string frontMatter, string markdown)
        {
            var metadata = new Dictionary<string, object>();
            _serializer.DeserializeInto(frontMatter, metadata);
            return new Slide
            {
                Metadata = metadata,
                Html = Markdig.Markdown.ToHtml(markdown, _pipeline)
            };
        }
    }
}
