using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

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
        private readonly RecordContext _context;

        /// <summary>
        /// Field to store bot service used to send messages.
        /// </summary>
        private readonly ITelegramBotService _bot;

        /// <summary>
        /// Holds string representations of various response results.
        /// </summary>
        private readonly Dictionary<ResponseState, string> _details;

        /// <summary>
        /// Constuctor that injects dependencies.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service to use.</param>
        /// <param name="options">Bandwidth options.</param>
        /// <param name="locale">Localization options.</param>
        public OwnApiService(RecordContext context, ITelegramBotService bot,
            IOptions<BandwidthOptions> options, IOptions<LocalizationOptions> locale)
        {
            _context = context;
            _bot = bot;

            _secondsPerRegen = options.Value.SecondsPerRegeneration;


            LocalizationOptions locOptions = locale.Value;
            _details = new Dictionary<ResponseState, string>()
            {
                [ResponseState.OkSent] = locOptions.RequestOk,
                [ResponseState.BandwidthExceeded] = locOptions.RequestBandwidthExceeded,
                [ResponseState.NoSuchToken] = locOptions.RequestNoToken,
                [ResponseState.SomethingBadHappened] = locOptions.RequestWhat
            };
        }

        /// <summary>
        /// Public method to handle incoming requests. Call underlying internal method
        /// and wraps its output into a Response.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        /// <returns>Response to the request, ready to be returned to client.</returns>
        public Response HandleRequest(Request request)
        {
            ResponseState result = HandleRequestInternally(request);
            return new Response(result, _details[result]);
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
            var record = _context.GetRecordByToken(request.Token);
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
