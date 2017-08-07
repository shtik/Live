using System.Collections.Generic;

namespace ShtikLive.Rendering.Markdown
{
    public class Slide
    {
        public Dictionary<string, object> Metadata { get; set; }
        public string Html { get; set; }
    }
}