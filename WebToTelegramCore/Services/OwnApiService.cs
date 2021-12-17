using System.Threading.Tasks;
using WebToTelegramCore.Exceptions;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that handles web API calls.
    /// </summary>
    public class OwnApiService : IOwnApiService
    {
        /// <summary>
        /// Field to store app's database context.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Field to store bot service used to send messages.
        /// </summary>
        private readonly ITelegramBotService _bot;

        /// <summary>
        /// Field to store Record management service.
        /// </summary>
        private readonly IRecordService _recordService;

        /// <summary>
        /// Constuctor that injects dependencies.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service to use.</param>
        /// <param name="options">Bandwidth options.</param>
        public OwnApiService(RecordContext context, ITelegramBotService bot, IRecordService recordService)
        {
            _context = context;
            _bot = bot;
            _recordService = recordService;
        }

        /// <summary>
        /// Public method to handle incoming requests. Call underlying internal method.
        /// </summary>
        /// <param name="request">Request to handle.</param>
        public async Task HandleRequest(Request request)
        {
            var record = await _context.GetRecordByToken(request.Token);
            if (record == null)
            {
                throw new TokenNotFoundException();
            }
            if (_recordService.CheckIfCanSend(record))
            {
                await _bot.Send(record.AccountNumber, request.Message, request.Silent, request.Type);
            }
            else
            {
                throw new BandwidthExceededException();
            }
        }
    }
}
