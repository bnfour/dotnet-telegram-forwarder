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
        /// <param name="silent">Flag to set whether to suppress the notification.</param>
        /// <param name="parsingType">Formatting type used in the message.</param>
        Task Send(long accountId, string message, bool silent, MessageParsingType parsingType);

        /// <summary>
        /// Sends a predefined sticker. Used as an easter egg with a 5% chance
        /// of message on unknown user input to be the sticker instead of text.
        /// </summary>
        /// <param name="accountId">ID of account to send sticker to.</param>
        Task SendTheSticker(long accountId);

        /// <summary>
        /// Sends message in CommonMark as Markdown. Used only internally as a crutch
        /// to display properly formatteded pre-defined messages. HTML breaks them :(
        /// </summary>
        /// <param name="accountId">ID of account to send message to.</param>
        /// <param name="message">Text of the message.</param>
        Task SendPureMarkdown(long accountId, string message);
    }
}
