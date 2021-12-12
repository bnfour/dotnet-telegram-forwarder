using WebToTelegramCore.Models;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Interface to implement non-Telegram web API handling.
    /// </summary>
    public interface IOwnApiService
    {
        /// <summary>
        /// Method to handle incoming request from the web API.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        // TODO add async everywhere and try not to go mad in the process
        void HandleRequest(Request request);
    }
}
