using System.Threading.Tasks;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.Interfaces
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
        Task HandleRequest(Request request);
    }
}
