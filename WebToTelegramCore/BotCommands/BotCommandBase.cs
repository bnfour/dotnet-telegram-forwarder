using WebToTelegramCore.Data;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands which are used to anything but to confirm or cancel
    /// pending destructive operations.
    /// </summary>
    public abstract class BotCommandBase : IBotCommand
    {
        // TODO: move to config
        /// <summary>
        /// Text somewhat explaining why processing of this Record
        /// was cancelled in this class.
        /// </summary>
        private const string _error = "You have an operation pending cancellation. " +
            "Please confirm or cancel it before using other commands.";

        /// <summary>
        /// Command text; not implemented in abstract classes.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Method of abstract base class that filters out users with pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(Record record)
        {
            return (record != null && record.State != RecordState.Normal) ?
                _error : null;
        }
    }
}
