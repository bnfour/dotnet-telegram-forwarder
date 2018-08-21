using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
        /// Constructor that also sets up the webhook.
        /// </summary>
        public TelegramBotService(IOptions<CommonOptions> options)
        {
            _client = new TelegramBotClient(options.Value.Token);
            // this code is dumb and single-threaded. _Maybe_ later
            // we can also probably set allowedUpdates to messages only
            // also how do i test this
            _client.SetWebhookAsync(options.Value.ApiEndpointUrl);
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
            _client.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
        }
    }
}
