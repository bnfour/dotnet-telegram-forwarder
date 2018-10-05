using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace WebToTelegramCore.FormatterHelpers
{
    /// <summary>
    /// HTML renderer for Markdig's LinkInline that does not put any formatting
    /// inside the link tag.
    /// </summary>
    public class NonNestedHtmlLinkInlineRenderer
        : Markdig.Renderers.Html.Inlines.LinkInlineRenderer
    {
        /// <summary>
        /// Method to render inline link.
        /// </summary>
        /// <param name="renderer">Renderer to use.</param>
        /// <param name="link">Link to render.</param>
        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(link.IsImage ? "<img src=\"" : "<a href=\"");
                renderer.WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url);
                renderer.Write("\"");
                renderer.WriteAttributes(link);
            }
            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" alt=\"");
                }
                var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;
                renderer.WriteChildren(link);
                renderer.EnableHtmlForInline = wasEnableHtmlForInline;
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write("\"");
                }
            }

            if (renderer.EnableHtmlForInline && !string.IsNullOrEmpty(link.Title))
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(link.Title);
                renderer.Write("\"");
            }

            if (link.IsImage)
            {
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write(" />");
                }
            }
            else
            {
                if (renderer.EnableHtmlForInline)
                {
                    if (AutoRelNoFollow)
                    {
                        renderer.Write(" rel=\"nofollow\"");
                    }
                    renderer.Write(">");
                }
                // this block of code was the sole reason of re-implemening this method
                // Telegram's API docs forbid nested HTML tags. Better safe than sorry.
                var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
                renderer.EnableHtmlForInline = false;
                renderer.WriteChildren(link);
                renderer.EnableHtmlForInline = wasEnableHtmlForInline;
                if (renderer.EnableHtmlForInline)
                {
                    renderer.Write("</a>");
                }
            }
        }
    }
}
