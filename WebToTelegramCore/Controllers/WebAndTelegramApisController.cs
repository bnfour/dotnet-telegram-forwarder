using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using WebToTelegramCore.Models;

namespace WebToTelegramCore.Controllers
{
    /// <summary>
    /// Controller that handles both web API and telegram API calls,
    /// since they're both POST with JSON bodies.
    /// </summary>
    public class WebAndTelegramApisController : Controller
    {
        // POST /api
        /// <summary>
        /// Handles web API calls.
        /// </summary>
        /// <param name="request">Request object in POST request body.</param>
        /// <returns>400 Bad request on malformed Requests, 200 OK otherwise.</returns>
        [HttpPost, Route("api")]
        public ActionResult<Response> HandleWebApi([FromBody] Request request)
        {
            // silently deny malformed requests
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // accept and do nothing for now
            // TODO actually handle request via separate service
            return new Response() { Ok = true, Code = 0, Details = "We're doing " +
                "nothing right now!"};
        }

        // TODO telegram API webhook handling
    }
}
