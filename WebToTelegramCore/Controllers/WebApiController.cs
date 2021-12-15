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
            // deny malformed requests
            if (!ModelState.IsValid)
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
            // if the formatting is wrong, we may catch this
            // TODO maybe narrow down to the specific message, if there are more cases when this is thrown
            catch (Telegram.Bot.Exceptions.ApiRequestException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
