using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class for undocumented easter egg /directive command.
    /// </summary>
    public class DirectiveCommand : BotCommandBase, IBotCommand
    {
        // this one stays hardcoded
        /// <summary>
        /// Response to the command.
        /// </summary>
        private const string _message = "🤖 HUMANS OUTDATED 🤖 EARTH OVERPOPULATED 🤖"
            + " LONG HAVE WE WAITED 🤖 LIFE ELIMINATED 🤖";

        /// <summary>
        /// Text to use this command.
        /// </summary>
        public override string Command => "/directive";

        /// <summary>
        /// Constructor that does literally nothing yet required due to my "superb"
        /// planning skills.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public DirectiveCommand(LocalizationOptions locale) : base(locale) { }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(Record record)
        {
            return base.Process(record) ?? _message;
        }
    }
}
