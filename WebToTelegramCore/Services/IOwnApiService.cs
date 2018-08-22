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
        /// <returns>Response to the request, ready to be returned to client.</returns>
        Response HandleRequest(Request request);
    }
}
