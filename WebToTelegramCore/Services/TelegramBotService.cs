using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Provides realization to ITelegramBotService. Encapsulates Telegram.Bot API
    /// to send messages.
    /// </summary>
    public class TelegramBotService : ITelegramBotService
    {
        /// <summary>
        /// Field to store bot client instance.
        /// </summary>
        private readonly TelegramBotClient _client;

        /// <summary>
        /// Field to store ID of the sticker to be used as an easter egg.
        /// </summary>
        private const string _theStickerID = "CAACAgIAAxkBAAIBLWRXhjYCsdyPBbQaJNEJlbtzpVKLAAJ6CQAC8UK_BfOoYI7o8z7PLwQ";

        /// <summary>
        /// Field to store our webhook URL to be advertised to Telegram's API.
        /// </summary>
        private readonly string _webhookUrl;

        /// <summary>
        /// Constructor that get the options required for this service to operate.
        /// </summary>
        /// <param name="options">Options to use.</param>
        public TelegramBotService(IOptions<CommonOptions> options)
        {
            _client = new TelegramBotClient(options.Value.Token);
            // made unclear that "api" part is needed as well, shot myself in the leg 3 years after
            _webhookUrl = options.Value.ApiEndpointUrl + "/api/" + options.Value.Token;
        }

        /// <summary>
        /// Method to manage external webhook for the Telegram API.
        /// Part one: called to set the webhook on application start.
        /// </summary>
        public async Task SetExternalWebhook()
        {
            await _client.SetWebhookAsync(_webhookUrl, allowedUpdates: new[] { UpdateType.Message });
        }

        /// <summary>
        /// Method to manage external webhook for the Telegram API.
        /// Part two: called to remove the webhook gracefully on application exit.
        /// </summary>
        public async Task ClearExternalWebhook()
        {
            await _client.DeleteWebhookAsync();
        }

        /// <summary>
        /// Method that is used from outside to send messages on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        /// <param name="message">Markdown-formatted message.</param>
        /// <param name="silent">Flag to set whether to suppress the notification.</param>
        /// <param name="parsingType">Formatting type used in the message.</param>
        public async Task Send(long accountId, string message, bool silent = false, MessageParsingType parsingType = MessageParsingType.Markdown)
        {
            // I think we have to promote account ID back to ID of chat with this bot
            var chatId = new ChatId(accountId);

            await _client.SendTextMessageAsync(chatId, message,
                ResolveRequestParseMode(parsingType), disableWebPagePreview: true,
                disableNotification: silent);
        }

        /// <summary>
        /// Method to send an arbitrary sticker on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        /// <param name="stickerFileId">ID of the sticker to send.</param>
        /// <param name="silent">Flag to set whether to suppress the notification.</param>
        public async Task SendSticker(long accountId, string stickerFileId, bool silent = false)
        {
            var chatId = new ChatId(accountId);
            var sticker = new InputOnlineFile(stickerFileId);

            await _client.SendStickerAsync(chatId, sticker, disableNotification: silent);
        }

        /// <summary>
        /// Method to send predefined sticker on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        public async Task SendTheSticker(long accountId)
            => await SendSticker(accountId, _theStickerID);

        private ParseMode? ResolveRequestParseMode(MessageParsingType fromRequest)
        {
            return fromRequest switch
            {
                MessageParsingType.Plaintext => null,
                MessageParsingType.Markdown => ParseMode.MarkdownV2,
                // should never happen, but for sake of completeness,
                // fall back to plaintext
                _ => null
            };
        }
    }
}
