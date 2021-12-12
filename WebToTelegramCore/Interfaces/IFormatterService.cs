namespace WebToTelegramCore.Interfaces
{
    /// <summary>
    /// Interface that exposes method to convert Markdown flavor that used in
    /// Telegram clients to HTML.
    /// </summary>
    public interface IFormatterService
    {
        /// <summary>
        /// Transforms Markdown to HTML. Expects Markdown like Telegram clients do:
        /// **bold** and __italic__ are main differences from the CommonMark.
        /// </summary>
        /// <param name="TgFlavoredMarkdown">Markdown-formatted text.</param>
        /// <returns>Text with same format but in HTML supported by Telegram's API.</returns>
        string TransformToHtml(string TgFlavoredMarkdown);
    }
}
