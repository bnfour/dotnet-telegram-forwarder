using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
        /// Constructor with dependency injection.
        /// </summary>
        /// <param name="ownApi">Web API service instance to use.</param>
        public WebAndTelegramApisController(IOwnApiService ownApi)
        {
            _ownApi = ownApi;
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

        // TODO telegram API webhook handling
    }
}
