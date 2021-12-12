using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /help command that should spew out some support text.
    /// </summary>
    public class HelpCommand : BotCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/help";

        /// <summary>
        /// Helpful help message to send.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor that sets up message.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public HelpCommand(LocalizationOptions locale) : base(locale)
        {
            _message = locale.Help;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="userId">Unused Telegram used ID.</param>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(long userId, Record record)
        {
            return base.Process(userId, record) ?? _message;
        }
    }
}
