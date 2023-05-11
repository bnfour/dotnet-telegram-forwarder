using System.Threading.Tasks;
using WebToTelegramCore.Data;

namespace WebToTelegramCore.Interfaces
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
        /// <param name="silent">Flag to set whether to suppress the notification.
        /// Like the API defaults to false for bot responses, which are sent immediately after user's message.</param>
        /// <param name="parsingType">Formatting type used in the message.
        /// Unlike the API, defaults for Markdown for bot responses, which do use some formatting.</param>
        Task Send(long accountId, string message, bool silent = false, MessageParsingType parsingType = MessageParsingType.Markdown);

        /// <summary>
        /// Method to send an arbitrary sticker on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        /// <param name="stickerFileId">ID of the sticker to send.</param>
        Task SendSticker(long accountId, string stickerFileId, bool silent = false);

        /// <summary>
        /// Sends a predefined sticker. Used as an easter egg with a 5% chance
        /// of message on unknown user input to be the sticker instead of text.
        /// </summary>
        /// <param name="accountId">ID of account to send sticker to.</param>
        Task SendTheSticker(long accountId);

        /// <summary>
        /// Method to manage external webhook for the Telegram API.
        /// Part one: called to set the webhook on application start.
        /// </summary>
        Task SetExternalWebhook();

        /// <summary>
        /// Method to manage external webhook for the Telegram API.
        /// Part two: called to remove the webhook gracefully on application exit.
        /// Not that it makes much sense in an app designed to be run 24/7, but is
        /// nice to have nonetheless.
        /// </summary>
        Task ClearExternalWebhook();
    }
}
