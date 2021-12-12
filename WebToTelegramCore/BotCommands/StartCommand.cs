using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /start command.
    /// </summary>
    public class StartCommand : BotCommandBase, IBotCommand
    {
        /// <summary>
        /// Fisrt part of response to the command which is always displayed.
        /// </summary>
        private readonly string _startMessage;

        /// <summary>
        /// Additional text to display when registration is open.
        /// </summary>
        private readonly string _registrationHint;

        /// <summary>
        /// Additional text to display when registration is closed.
        /// </summary>
        private readonly string _noRegistration;

        /// <summary>
        /// Field to store current state of registartion of new users.
        /// </summary>
        private readonly bool _isRegistrationOpen;

        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/start";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        /// <param name="registrationEnabled">Boolean indicating whether registration
        /// is enabled (true) or not.</param>
        public StartCommand(LocalizationOptions locale, bool registrationEnabled)
            : base(locale)
        {
            _isRegistrationOpen = registrationEnabled;

            _startMessage = locale.StartMessage;
            _noRegistration = locale.StartGoAway;
            _registrationHint = locale.StartRegistrationHint;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="userId">Unused Telegram user ID.</param>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(long userId, Record record)
        {
            string appendix = _isRegistrationOpen ? _registrationHint : _noRegistration;
            return base.Process(userId, record) ?? _startMessage + "\n\n" + appendix;
        }
    }
}
