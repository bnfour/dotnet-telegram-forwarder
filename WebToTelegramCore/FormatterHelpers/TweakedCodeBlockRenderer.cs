using Markdig.Renderers;
using Markdig.Syntax;

namespace WebToTelegramCore.FormatterHelpers
{
    /// <summary>
    /// Class that renders multiline code blocks inside &lt;pre&gt; tags only
    /// as requested by Telegram's API docs,
    /// opposed to default &lt;pre&gt; and &lt;code&gt;.
    /// </summary>
    public class TweakedCodeBlockRenderer : Markdig.Renderers.Html.CodeBlockRenderer
    {
        /// <summary>
        /// Method to render the code block.
        /// </summary>
        /// <param name="renderer">Renderer to use.</param>
        /// <param name="obj">Code block to render.</param>
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            renderer.EnsureLine();
            // all code related to <div> was wiped out as it's unused here anyway
            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<pre");
                renderer.WriteAttributes(obj);
                renderer.Write(">");
            }

            renderer.WriteLeafRawLines(obj, true, true);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.WriteLine("</pre>");
            }

            renderer.EnsureLine();

        }
    }
}
