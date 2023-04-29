using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands that are both not confirmation commands and
    /// available only to users which do not have a token yet.
    /// </summary>
    public abstract class GuestOnlyCommandBase : BotCommandBase, IBotCommand
    {
        /// <summary>
        /// Constructor that sets up error message.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public GuestOnlyCommandBase() : base() { }

        /// <summary>
        /// Method of abstract base class that adds filtering out users
        /// with no associated token. </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending or user has a token,
        /// or null otherwise.</returns>
        public new virtual string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Method that filters out users with tokens.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if user has a token, or null otherwise.</returns>
        private string InternalProcess(Record record)
        {
            return string.IsNullOrEmpty(record.Token)
                ? null
                : LocalizationOptions.ErrorMustBeGuest;
        }
    }
}
