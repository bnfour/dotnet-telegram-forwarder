using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using WebToTelegramCore.Exceptions;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.Controllers
{
    public class WebApiController : Controller
    {
        /// <summary>
        /// Field to store injected web API service.
        /// </summary>
        private readonly IOwnApiService _ownApi;

        public WebApiController(IOwnApiService ownApi)
        {
            _ownApi = ownApi;
        }

        // POST /api
        /// <summary>
        /// Handles web API calls.
        /// </summary>
        /// <param name="request">Request object in POST request body.</param>
        /// <returns>HTTP status code result indicating whether the request was handled
        /// successfully, or one of the error codes.</returns>
        [HttpPost, Route("api")]
        public async Task<ActionResult> HandleWebApi([FromBody] Request request)
        {
            // deny malformed requests as detected by binder, or by "business" logic in here
            if (!ModelState.IsValid || !IsValid(request))
            {
                return BadRequest();
            }
            try
            {
                await _ownApi.HandleRequest(request);
                return Ok();
            }
            catch (TokenNotFoundException)
            {
                return NotFound();
            }
            catch (BandwidthExceededException)
            {
                return StatusCode((int)HttpStatusCode.TooManyRequests);
            }
            // if the formatting is malformed, relay Telegram's "bad request" to the user
            catch (Telegram.Bot.Exceptions.ApiRequestException ex) when (ex.Message.StartsWith("Bad Request"))
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // note that malformed markdown or invalid IDs are caught by Telegram backend,
        // this merely checks that the fields are set correctly, not the contents

        /// <summary>
        /// Checks for validity not caught by model binder.
        /// Assures that one of either <see cref="Request.Message"/>
        /// or <see cref="Request.Sticker"/> is set.
        /// </summary>
        /// <param name="request">Request object.</param>
        /// <returns>Whether the request is valid by its fields.</returns>
        private bool IsValid(Request request)
        {
            var hasMessage = !string.IsNullOrEmpty(request.Message);
            var hasSticker = !string.IsNullOrEmpty(request.Sticker);
            // either a message or a sticker, not both, not neither
            return hasMessage ^ hasSticker;
        }
    }
}
