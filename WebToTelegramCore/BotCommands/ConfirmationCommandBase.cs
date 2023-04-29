using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for confirmation commands that is used only when
    /// a destructive operation is pending for a given user. That means this user
    /// must have a token since all destructive operations are tied to token.
    /// </summary>
    public abstract class ConfirmationCommandBase : IBotCommand
    {
        /// <summary>
        /// Command text; not implemented in abstract classes.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfirmationCommandBase() { }

        /// <summary>
        /// Method of abstract base class that filters out users without pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is no operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(Record record)
        {
            return (string.IsNullOrEmpty(record.Token) || record.State == RecordState.Normal)
                ? LocalizationOptions.ErrorNoConfirmationPending
                : null;
        }
    }
}
