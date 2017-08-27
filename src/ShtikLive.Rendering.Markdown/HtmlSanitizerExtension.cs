using System.Linq;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ShtikLive.Rendering.Markdown
{
    public class HtmlSanitizerExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                var blockRenderer = htmlRenderer.ObjectRenderers.FindExact<HtmlBlockRenderer>();
                if (blockRenderer != null)
                {
                    blockRenderer.TryWriters.Remove(TryScriptBlockRenderer);
                    blockRenderer.TryWriters.Add(TryScriptBlockRenderer);
                }

                var inlineRenderer = htmlRenderer.ObjectRenderers.FindExact<HtmlInlineRenderer>();
                if (inlineRenderer != null)
                {
                    inlineRenderer.TryWriters.Remove(TryScriptInlineRenderer);
                    inlineRenderer.TryWriters.Add(TryScriptInlineRenderer);
                }
            }
        }

        private bool TryScriptInlineRenderer(HtmlRenderer renderer, HtmlInline inline)
        {
            if (inline.Tag.Contains("script"))
            {
                renderer.WriteEscape(inline.Tag);
                return true;
            }
            return false;
        }

        private bool TryScriptBlockRenderer(HtmlRenderer renderer, HtmlBlock block)
        {
            if (block.Lines.Lines.Any(l => l.Slice.Text.Contains("script")))
            {
                foreach (var line in block.Lines.Lines)
                {
                    renderer.WriteEscape(line.Slice);
                }
                return true;
            }
            return false;
        }
    }
}