using Update = Telegram.Bot.Types.Update;

namespace WebToTelegramCore.Interfaces
{
    /// <summary>
    /// Interface to implement Telegram webhook handling.
    /// </summary>
    public interface ITelegramApiService
    {
        /// <summary>
        /// Checks whether passed string is an actual bot token to verify the request
        /// actully comes from Telegram backend.
        /// </summary>
        /// <param name="calledToken">Token received from request.</param>
        /// <returns>True if calledToken is actual token, false otherwise.</returns>
        bool IsToken(string calledToken);

        /// <summary>
        /// Method to handle incoming updates from the webhook.
        /// </summary>
        /// <param name="update">Received update.</param>
        void HandleUpdate(Update update);
    }
}
