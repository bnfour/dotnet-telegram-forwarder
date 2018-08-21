namespace WebToTelegramCore.Options
{
    /// <summary>
    /// Represents application-wide options.
    /// </summary>
    public class CommonOptions
    {
        /// <summary>
        /// Telegram API bot token. Used both to receive (for auth) and to send
        /// Telegram messages.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Absolute URL to API endpoint, not including the token. Used to set webhook.
        /// </summary>
        public string ApiEndpointUrl { get; set; }
    }
}
