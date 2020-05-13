using WebToTelegramCore.Data;
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
        /// Text somewhat explaining why processing of this Record
        /// was cancelled in this class.
        /// </summary>
        private readonly string _error;

        /// <summary>
        /// Command text; not implemented in abstract classes.
        /// </summary>
        public abstract string Command { get; }

        /// <summary>
        /// Constructor that sets error message.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public ConfirmationCommandBase(LocalizationOptions locale)
        {
            _error = locale.ErrorNoConfirmationPending;
        }

        /// <summary>
        /// Method of abstract base class that filters out users without pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="userId">Unused Telegram user ID.</param>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is no operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(long userId, Record record)
        {
            return (record == null || record.State == RecordState.Normal) ?
                _error : null;
        }
    }
}
