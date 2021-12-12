using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
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
        /// Field to store ID of the sticker to be used as an easter egg
        /// </summary>
        private const string _theStickerID = "CAADAgADegkAAvFCvwXzqGCO6PM-zwI";

        /// <summary>
        /// Wrapping of sticker ID understandable by the API. We're sending sticker
        /// that is already in the cloud.
        /// </summary>
        private readonly InputOnlineFile _sticker = new InputOnlineFile(_theStickerID);

        /// <summary>
        /// Field to store used instance of formatter.
        /// </summary>
        private readonly IFormatterService _formatter;

        /// <summary>
        /// Constructor that also sets up the webhook.
        /// </summary>
        /// <param name="options">Options to use.</param>
        /// <param name="formatter">Formatter to use.</param>
        public TelegramBotService(IOptions<CommonOptions> options,
            IFormatterService formatter)
        {
            _client = new TelegramBotClient(options.Value.Token);

            _formatter = formatter;

            // made unclear that "api" part is needed as well, shot myself in the leg 3 years after
            var webhookUrl = options.Value.ApiEndpointUrl + "/api/" + options.Value.Token;
            // don't know whether old version without firing an actual thread still worked,
            // it was changed as i tried to debug a "connection" issue where wrong config file was loaded
            Task.Run(() => _client.SetWebhookAsync(webhookUrl,
               allowedUpdates: new[] { UpdateType.Message })).Wait();
        }

        /// <summary>
        /// Destructor that removes the webhook.
        /// </summary>
        ~TelegramBotService()
        {
            // this probably never worked anyway
            // TODO think of a better way to acquire/release webhooks
            Task.Run(() => _client.DeleteWebhookAsync()).Wait();
        }

        /// <summary>
        /// Method that is used from outside to send messages on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        /// <param name="message">Markdown-formatted message.</param>
        public async Task Send(long accountId, string message, bool silent)
        {
            // I think we have to promote account ID back to ID of chat with this bot
            var chatId = new ChatId(accountId);
            await _client.SendTextMessageAsync(chatId, _formatter.TransformToHtml(message),
                ParseMode.Html, disableWebPagePreview: true, disableNotification: silent);
        }

        /// <summary>
        /// Method to send predefined sticker on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        public async Task SendTheSticker(long accountId)
        {
            var chatId = new ChatId(accountId);
            await _client.SendStickerAsync(chatId, _sticker);
        }

        /// <summary>
        /// Sends message in CommonMark as Markdown. Used only internally as a crutch
        /// to display properly formatteded pre-defined messages. HTML breaks them :(
        /// </summary>
        /// <param name="accountId">ID of account to send message to.</param>
        /// <param name="message">Text of the message.</param>
        public async Task SendPureMarkdown(long accountId, string message)
        {
            var chatId = new ChatId(accountId);
            await _client.SendTextMessageAsync(chatId, message, ParseMode.Markdown, disableWebPagePreview: true);
        }
    }
}
