namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Interface that exposes greatly simplified version of Telegram bot API.
    /// The only method allows sending of text messages.
    /// </summary>
    public interface ITelegramBotService
    {
        /// <summary>
        /// Sends text message. Markdown formatting should be supported.
        /// </summary>
        /// <param name="accountId">ID of account to send message to.</param>
        /// <param name="message">Text of the message.</param>
        void Send(int accountId, string message);
    }
}
