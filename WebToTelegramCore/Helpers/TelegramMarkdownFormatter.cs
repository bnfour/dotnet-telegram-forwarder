namespace WebToTelegramCore.Helpers
{
    /// <summary>
    /// Static helper to make not hardcoded texts suitable for sending.
    /// </summary>
    public static class TelegramMarkdownFormatter
    {
        /// <summary>
        /// List of characters to escape in order to satisfy the API.
        /// </summary>
        private readonly static string[] _toEscape = new[]
        {
            "_", "*", "[", "]", "(", ")", "~", "`", ">",
            "#", "+", "-", "=", "|", "{", "}", ".", "!" 
        };

        /// <summary>
        /// Escapes the dangerous symbols in the string.
        /// </summary>
        /// <param name="s">String to process.</param>
        /// <returns>Telegram Markdown v2 friendly string.</returns>
        public static string Escape(string s)
        {
            foreach (var c in _toEscape)
            {
                s = s.Replace(c, @"\" + c);
            }
            return s;
        }
    }
}
