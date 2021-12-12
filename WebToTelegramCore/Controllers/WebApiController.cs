using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebToTelegramCore.Models;
using WebToTelegramCore.Services;

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
        /// <returns>400 Bad request on malformed Requests,
        /// 200 OK with corresponding Response otherwise.</returns>
        [HttpPost, Route("api")]
        // TODO drop response from return type
        public async Task<ActionResult<Response>> HandleWebApi([FromBody] Request request)
        {
            // silently deny malformed requests
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return _ownApi.HandleRequest(request);
        }
    }
}
