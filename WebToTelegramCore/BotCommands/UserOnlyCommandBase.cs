using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands that are both not confirmation commands and
    /// available only to users which already have a token.
    /// </summary>
    public abstract class UserOnlyCommandBase : BotCommandBase, IBotCommand
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserOnlyCommandBase() : base() { }

        /// <summary>
        /// Method of abstract base class that adds filtering out users
        /// with no associated token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending or user has no token,
        /// or null otherwise.</returns>
        public new virtual string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Method that filters out users with no tokens.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if user has no token,
        /// or null otherwise.</returns>
        private string InternalProcess(Record record)
        {
            return string.IsNullOrEmpty(record.Token) ? Locale.ErrorMustBeUser : null;
        }
    }
}
