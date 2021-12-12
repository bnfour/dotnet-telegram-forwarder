using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WebToTelegramCore.Interfaces;

namespace WebToTelegramCore.Controllers
{
    /// <summary>
    /// Controller that handles both web API and telegram API calls,
    /// since they're both POST with JSON bodies.
    /// </summary>
    public class TelegramApiController : Controller
    {
        /// <summary>
        /// Field to store injected Telegram API service.
        /// </summary>
        private ITelegramApiService _tgApi;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="ownApi">Web API service instance to use.</param>
        /// <param name="tgApi">Telegram API service instance to use.</param>
        public TelegramApiController(ITelegramApiService tgApi)
        {
            _tgApi = tgApi;
        }

        // POST /api/{bot token}
        /// <summary>
        /// Handles webhook calls.
        /// </summary>
        /// <param name="token">Token which is used as part of endpoint url
        /// to verify request's origin.</param>
        /// <param name="update">Update to handle.</param>
        /// <returns>404 Not Found on wrong tokens, 200 OK otherwise,
        /// unless there is an internal server error.</returns>
        [HttpPost, Route("api/{token}")]
        public async Task<ActionResult> HandleTelegramApi(string token, [FromBody] Update update)
        {
            if (!_tgApi.IsToken(token))
            {
                return NotFound();
            }
            try
            {
                await _tgApi.HandleUpdate(update);
                return Ok();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
