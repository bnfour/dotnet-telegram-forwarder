﻿using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

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

            var webhookUrl = options.Value.ApiEndpointUrl + "/" + options.Value.Token;
            // this code is dumb and single-threaded. _Maybe_ later
            _client.SetWebhookAsync(webhookUrl,
                allowedUpdates: new[] { UpdateType.Message });
        }

        /// <summary>
        /// Destructor that removes the webhook.
        /// </summary>
        ~TelegramBotService()
        {
            _client.DeleteWebhookAsync();
        }

        /// <summary>
        /// Method that is used from outside to send messages on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        /// <param name="message">Markdown-formatted message.</param>
        public void Send(long accountId, string message)
        {
            // I think we have to promote account ID back to ID of chat with this bot
            var chatId = new ChatId(accountId);
            _client.SendTextMessageAsync(chatId, _formatter.TransformToHtml(message),
                ParseMode.Html, true);
        }

        /// <summary>
        /// Method to send predefined sticker on behalf of the bot.
        /// </summary>
        /// <param name="accountId">ID of the account to send to.</param>
        public void SendTheSticker(long accountId)
        {
            var chatId = new ChatId(accountId);
            _client.SendStickerAsync(chatId, _sticker);
        }

        /// <summary>
        /// Sends message in CommonMark as Markdown. Used only internally as a crutch
        /// to display properly formatteded pre-defined messages. HTML breaks them :(
        /// </summary>
        /// <param name="accountId">ID of account to send message to.</param>
        /// <param name="message">Text of the message.</param>
        public void SendPureMarkdown(long accountId, string message)
        {
            var chatId = new ChatId(accountId);
            _client.SendTextMessageAsync(chatId, message, ParseMode.Markdown, true);
        }
    }
}
