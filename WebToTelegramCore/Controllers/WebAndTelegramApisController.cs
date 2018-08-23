using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

using WebToTelegramCore.Models;
using WebToTelegramCore.Services;

namespace WebToTelegramCore.Controllers
{
    /// <summary>
    /// Controller that handles both web API and telegram API calls,
    /// since they're both POST with JSON bodies.
    /// </summary>
    public class WebAndTelegramApisController : Controller
    {
        /// <summary>
        /// Field to store injected web API service.
        /// </summary>
        private IOwnApiService _ownApi;

        /// <summary>
        /// Field to store injected Telegram API service.
        /// </summary>
        private ITelegramApiService _tgApi;

        /// <summary>
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="ownApi">Web API service instance to use.</param>
        /// <param name="tgApi">Telegram API service instance to use.</param>
        public WebAndTelegramApisController(IOwnApiService ownApi,
            ITelegramApiService tgApi)
        {
            _ownApi = ownApi;
            _tgApi = tgApi;
        }

        // POST /api
        /// <summary>
        /// Handles web API calls.
        /// </summary>
        /// <param name="request">Request object in POST request body.</param>
        /// <returns>400 Bad request on malformed Requests,
        /// 200 OK with corresponding Response otherwise.</returns>
        [HttpPost, Route("api")]
        public ActionResult<Response> HandleWebApi([FromBody] Request request)
        {
            // silently deny malformed requests
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return _ownApi.HandleRequest(request);
        }

        // POST /api/{bot token}
        /// <summary>
        /// Handles webhook calls.
        /// </summary>
        /// <param name="token">Token which is used as part of endpoint url
        /// to verify request's origin.</param>
        /// <param name="update">Update to handle.</param>
        /// <returns>400 Bad Request on wrong tokens, 200 OK otherwise.</returns>
        [HttpPost, Route("api/{token}")]
        public ActionResult HandleTelegramApi(string token, [FromBody] Update update)
        {
            if (!_tgApi.IsToken(token))
            {
                return BadRequest();
            }
            _tgApi.HandleUpdate(update);
            return Ok();
        }
    }
}
