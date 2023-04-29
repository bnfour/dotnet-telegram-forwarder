using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands which are used to anything but to confirm or cancel
    /// pending destructive operations.
    /// </summary>
    public abstract class BotCommandBase : IBotCommand
    {
        /// <summary>
        /// Command text; not implemented in abstract classes.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BotCommandBase() { }

        /// <summary>
        /// Method of abstract base class that filters out users with pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(Record record)
        {
            return (!string.IsNullOrEmpty(record.Token) && record.State != RecordState.Normal)
                ? Locale.ErrorConfirmationPending
                : null;
        }
    }
}
