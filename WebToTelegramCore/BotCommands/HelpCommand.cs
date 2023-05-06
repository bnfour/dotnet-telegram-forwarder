using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

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
        /// Constructor.
        /// </summary>
        public HelpCommand() : base() { }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(Record record)
        {
            return base.Process(record) ?? Locale.Help;
        }
    }
}
