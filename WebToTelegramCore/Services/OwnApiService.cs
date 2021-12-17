using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebToTelegramCore.Exceptions;
using WebToTelegramCore.Interfaces;
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
        /// Constuctor that injects dependencies.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service to use.</param>
        /// <param name="options">Bandwidth options.</param>
        public OwnApiService(RecordContext context, ITelegramBotService bot, IOptions<BandwidthOptions> options)
        {
            _context = context;
            _bot = bot;

            _secondsPerRegen = options.Value.SecondsPerRegeneration;

        }

        /// <summary>
        /// Public method to handle incoming requests. Call underlying internal method.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        public async Task HandleRequest(Request request)
        {
            await HandleRequestInternally(request);
        }

        /// <summary>
        /// Internal method to handle requests.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        private async Task HandleRequestInternally(Request request)
        {
            var record = await _context.GetRecordByToken(request.Token);
            if (record == null)
            {
                throw new TokenNotFoundException();
            }
            UpdateRecordCounter(record);
            if (record.UsageCounter > 0)
            {
                record.LastSuccessTimestamp = DateTime.Now;
                record.UsageCounter--;
                await _bot.Send(record.AccountNumber, request.Message, request.Silent, request.Type);
            }
            else
            {
                throw new BandwidthExceededException();
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
