using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /start command.
    /// </summary>
    public class StartCommand : BotCommandBase, IBotCommand
    {
        // TODO: move to config
        /// <summary>
        /// Fisrt part of response to the command which is always displayed.
        /// </summary>
        private const string _startMessage = "Hello!\n\n" +
            "This bot provides a standalone web API to relay messages from whatever " +
            "you'll use it from to Telegram as messages from the bot." +
            "It might come in handy to unify your notifications in one place.\n\n" +
            "*Please note*: this requires some external tools. If you consider " +
            "phrases like \"Send a POST request to the endpoint with JSON body " +
            "with two string fields\" a magic gibberish you don't understand, " +
            "this bot probably isn't much of use to you.";

        /// <summary>
        /// Additional text to display when registration is open.
        /// </summary>
        private const string _registrationHint = "If that does not stop you, " +
            "you can create your own API token with /create command.";

        /// <summary>
        /// Additional text to display when registration is closed.
        /// </summary>
        private const string _noRegistration = "Sorry, this instance of bot is not " +
            "accepting new users for now.";

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
        public StartCommand(bool registrationEnabled)
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
            string appendix = _isRegistrationOpen ? _registrationHint : _noRegistration;
            return base.Process(record) ?? _startMessage + "\n\n" + appendix;
        }
    }
}
