namespace WebToTelegramCore.Data
{
    /// <summary>
    /// Represents available parsing modes for the user message
    /// to be sent via Telegram's API.
    /// </summary>
    public enum MessageParsingType
    {
        /// <summary>
        /// Text as is, no formatting.
        /// </summary>
        Plaintext,
        /// <summary>
        /// Text with some formatting to be displayed.
        /// Please note that this is Telegram's own "MarkdownV2" flavour,
        /// see https://core.telegram.org/bots/api#markdownv2-style for reference.
        /// </summary>
        Markdown
    }
}
