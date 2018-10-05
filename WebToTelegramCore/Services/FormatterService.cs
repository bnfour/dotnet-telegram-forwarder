using System.IO;
using System.Linq;
using WebToTelegramCore.FormatterHelpers;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that converts Markdown to HTML.
    /// </summary>
    public class FormatterService : IFormatterService
    {
        /// <summary>
        /// Instance of MarkdownTransformer to use.
        /// </summary>
        private readonly MarkdownTransformer _transformer = new MarkdownTransformer();

        /// <summary>
        /// Transforms Markdown to HTML. Expects Markdown like Telegram clients do:
        /// **bold** and __italic__ are main differences from the CommonMark.
        /// </summary>
        /// <param name="TgFlavoredMarkdown">Markdown-formatted text.</param>
        /// <returns>Text with same format but in HTML supported by Telegram's API.</returns>
        public string TransformToHtml(string TgFlavoredMarkdown)
        {
            // TODO think how to reuse these
            TextWriter writer = new StringWriter();
            var renderer = new Markdig.Renderers.HtmlRenderer(writer);

            renderer = new Markdig.Renderers.HtmlRenderer(writer)
            {
                ImplicitParagraph = true
            };

            // replacing default renderes with our "fixed" versions
            var toRemove = renderer.ObjectRenderers.Where(x =>
                x.GetType() == typeof(Markdig.Renderers.Html.CodeBlockRenderer) ||
                x.GetType() == typeof(Markdig.Renderers.Html.Inlines.LinkInlineRenderer))
            .ToList();
            foreach (var r in toRemove)
            {
                renderer.ObjectRenderers.Remove(r);
            }
            renderer.ObjectRenderers.Add(new NonNestedHtmlLinkInlineRenderer());
            renderer.ObjectRenderers.Add(new TweakedCodeBlockRenderer());

            // setting up rendering pipeline
            var pipeline = new Markdig.MarkdownPipelineBuilder().Build();
            pipeline.Setup(renderer);

            // actually rendering the HTML
            var cm = _transformer.ToCommonMark(TgFlavoredMarkdown);
            var doc = Markdig.Parsers.MarkdownParser.Parse(cm);
            renderer.Render(doc);
            writer.Flush();
            var formatted = writer.ToString();
            formatted = formatted
                .Replace("<br />", "&#10;");
            return formatted;
        }
    }
}
