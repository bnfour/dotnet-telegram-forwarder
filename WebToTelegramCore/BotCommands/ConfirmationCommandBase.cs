using WebToTelegramCore.Data;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for confirmation commands that is used only when
    /// a destructive operation is pending for a given user. That means this user
    /// must have a token since all destructive operations are tied to token.
    /// </summary>
    public abstract class ConfirmationCommandBase : IBotCommand
    {
        // TODO: move to config
        /// <summary>
        /// Text somewhat explaining why processing of this Record
        /// was cancelled in this class.
        /// </summary>
        private const string _error = "This command is only usable when you have " +
            "initiated a token deletion /delete or regeneration /regenerate.";

        /// <summary>
        /// Command text; not implemented in abstract classes.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Method of abstract base class that filters out users without pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is no operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(Record record)
        {
            return (record == null || record.State == RecordState.Normal) ?
                _error : null;
        }
    }
}
