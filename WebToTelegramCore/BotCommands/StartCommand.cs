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
        /// <param name="registrationEnabled">Boolean indicating whether registration
        /// is enabled (true) or not.</param>
        public StartCommand(bool registrationEnabled) : base()
        {
            _isRegistrationOpen = registrationEnabled;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(Record record)
        {
            string appendix = _isRegistrationOpen ? LocalizationOptions.StartRegistrationHint : LocalizationOptions.StartGoAway;
            return base.Process(record) ?? LocalizationOptions.StartMessage + "\n\n" + appendix;
        }
    }
}
