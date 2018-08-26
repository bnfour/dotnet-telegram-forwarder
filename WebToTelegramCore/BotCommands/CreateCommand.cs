using System;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;
using WebToTelegramCore.Services;

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
        /// Message to display on token creation. Must be formatted, {0} is token.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Message to display when registration is off.
        /// </summary>
        private readonly string _goAway;

        /// <summary>
        /// Field to store database context reference.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Field to store token generator reference.
        /// </summary>
        private readonly ITokenGeneratorService _generator;

        // I'm bad at computer programming:
        // the existing "architecture" always passes nulls as Records if user have
        // no record in DB yet. That means it's impossible to create a user :(
        // so here we go
        // TODO: do something better
        /// <summary>
        /// ID of account that sent the command.
        /// </summary>
        public long? Crutch { get; set; }

        /// <summary>
        /// Field to store whether registration is enabled. True is enabled.
        /// </summary>
        private readonly bool _isRegistrationEnabled;

        /// <summary>
        /// Constructor that injects dependencies and sets up registration state.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        /// <param name="context">Database context to use.</param>
        /// <param name="generator">Token generator service to use.</param>
        /// <param name="isRegistrationEnabled">State of registration.</param>
        public CreateCommand(LocalizationOptions locale, RecordContext context,
            ITokenGeneratorService generator, bool isRegistrationEnabled) : base(locale)
        {
            _context = context;
            _generator = generator;
            _isRegistrationEnabled = isRegistrationEnabled;

            _message = locale.CreateSuccess;
            _goAway = locale.CreateGoAway;
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
        /// <param name="record">Record to process. _Must be null_.</param>
        /// <returns>Message with new token or message stating that registration
        /// is closed for good.</returns>
        private string InternalProcess(Record record)
        {
            // record being null is enforced by base calls.
            if (!Crutch.HasValue)
            {
                throw new ApplicationException("Crutch is not set before calling");
            }
            if (_isRegistrationEnabled)
            {
                string token = _generator.Generate();
                Record r = new Record() { AccountNumber = Crutch.Value, Token = token };
                _context.Add(r);
                _context.SaveChanges();
                return String.Format(_message, token);
            }
            else
            {
                return _goAway;
            }
        }
    }
}
