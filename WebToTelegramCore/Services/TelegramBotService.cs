using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

        // TODO: move to config and load API token as well as our API endpoint for webhook
        private const string _token = "some seecret token";

        /// <summary>
        /// Constructor that also set up webhook.
        /// </summary>
        public TelegramBotService()
        {
            _client = new TelegramBotClient(_token);
            // this code is dumb and single-threaded. _Maybe_ later
            _client.SetWebhookAsync("API endpoint URL with token in it " +
                "to make sure it's real update");
        }

        /// <summary>
        /// Destructor that removes webhook.
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
        public void Send(int accountId, string message)
        {
            // I think we have to promote account ID back to ID of chat with this bot
            // TODO: convert argument and account ID elsewhere to long
            var chatId = new ChatId(accountId);
            _client.SendTextMessageAsync(chatId, message, ParseMode.Markdown);
        }
    }
}
