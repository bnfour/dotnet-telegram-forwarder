using Microsoft.Extensions.Options;
using System;
using System.Linq;

using WebToTelegramCore.Data;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that handles web API calls.
    /// </summary>
    public class OwnApiService : IOwnApiService
    {
        /// <summary>
        /// Amount of seconds lince last successful API call to regenerate counter.
        /// </summary>
        private readonly int _secondsPerRegen;

        /// <summary>
        /// Field to store app's database context.
        /// </summary>
        private RecordContext _context;

        /// <summary>
        /// Field to store bot service used to send messages.
        /// </summary>
        private ITelegramBotService _bot;

        /// <summary>
        /// Constuctor that injects dependencies.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service to use.</param>
        /// <param name="options">Bandwidth options.</param>
        public OwnApiService(RecordContext context, ITelegramBotService bot,
            IOptions<BandwidthOptions> options)
        {
            _context = context;
            _bot = bot;

            _secondsPerRegen = options.Value.SecondsPerRegeneration;
        }

        /// <summary>
        /// Public method to handle incoming requests. Call underlying internal method
        /// and wraps its output into a Response.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>Response to the request, ready to be returned to client.</returns>
        public Response HandleRequest(Request request)
        {
            return new Response(HandleRequestInternally(request));
        }

        /// <summary>
        /// Internal method to handle requests.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>ResponseState indicatig result of the request processing.</returns>
        private ResponseState HandleRequestInternally(Request request)
        {
            // wrapping all these into a big try clause
            // returning ResponseState.SomethingBadHappened in catch is silly
            // but how else API can be made "robust"?
            // TODO: think about role of ResponseState.SomethingBadHappened
            var record = GetRecordByToken(request.Token);
            if (record == null)
            {
                return ResponseState.NoSuchToken;
            }
            UpdateRecordCounter(record);
            if (record.UsageCounter > 0)
            {
                record.LastSuccessTimestamp = DateTime.Now;
                record.UsageCounter--;
                _bot.Send(record.AccountNumber, request.Message);
                return ResponseState.OkSent;
            }
            else
            {
                return ResponseState.BandwidthExceeded;
            }
        }

        /// <summary>
        /// Tries to fetch record by token from underlying context.
        /// </summary>
        /// <param name="token">Token to search for in the DB.</param>
        /// <returns>Associated Record or null if none found.</returns>
        private Record GetRecordByToken(string token)
        {
            Record ret;
            try
            {
                ret = _context.Records.Single(r => r.Token.Equals(token));
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return ret;
        }

        /// <summary>
        /// Updates Record's usage counter based on time passed since last successful
        /// message delivery.
        /// </summary>
        /// <param name="record">Record to update.</param>
        private void UpdateRecordCounter(Record record)
        {
            TimeSpan sinceLastSuccess = DateTime.Now - record.LastSuccessTimestamp;
            int toAdd = (int)(sinceLastSuccess.TotalSeconds / _secondsPerRegen);
            // overflows are handled in the property's setter
            record.UsageCounter += toAdd;
        }
    }
}
