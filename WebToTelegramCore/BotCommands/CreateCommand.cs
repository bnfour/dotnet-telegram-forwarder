using System;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// /create command, allows user to create a token and start using the bot.
    /// </summary>
    public class CreateCommand : GuestOnlyCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/create";

        /// <summary>
        /// Field to store database context reference.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Field to store token generator reference.
        /// </summary>
        private readonly ITokenGeneratorService _generator;

        /// <summary>
        /// Record manipulation service helper reference.
        /// </summary>
        private readonly IRecordService _recordService;

        /// <summary>
        /// Field to store whether registration is enabled. True is enabled.
        /// </summary>
        private readonly bool _isRegistrationEnabled;

        /// <summary>
        /// Constructor that injects dependencies and sets up registration state.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="generator">Token generator service to use.</param>
        /// <param name="recordService">Record helper service to use.</param>
        /// <param name="isRegistrationEnabled">State of registration.</param>
        public CreateCommand(RecordContext context, ITokenGeneratorService generator,
            IRecordService recordService, bool isRegistrationEnabled) : base()
        {
            _context = context;
            _generator = generator;
            _recordService = recordService;

            _isRegistrationEnabled = isRegistrationEnabled;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Message with new token or error when there is one already.</returns>
        public override string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Actual method that does registration or denies it.
        /// </summary>
        /// <param name="record">Record to process. Is null if working properly.</param>
        /// <returns>Message with new token or message stating that registration
        /// is closed for good.</returns>
        private string InternalProcess(Record record)
        {
            if (_isRegistrationEnabled)
            {
                string token = _generator.Generate();
                var r = _recordService.Create(token, record.AccountNumber);
                _context.Add(r);
                _context.SaveChanges();
                return String.Format(Locale.CreateSuccess, token);
            }
            else
            {
                return Locale.CreateGoAway;
            }
        }
    }
}
