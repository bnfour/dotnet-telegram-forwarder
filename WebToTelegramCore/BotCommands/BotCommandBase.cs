using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands which are used to anything but to confirm or cancel
    /// pending destructive operations.
    /// </summary>
    public abstract class BotCommandBase : IBotCommand
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
        public BotCommandBase(LocalizationOptions locale)
        {
            _error = locale.ErrorConfirmationPending;
        }

        /// <summary>
        /// Method of abstract base class that filters out users with pending
        /// cancellations or deletions of token.
        /// </summary>
        /// <param name="userId">Telegram ID of user that sent the message.</param>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending,
        /// or null otherwise.</returns>
        public virtual string Process(long userId, Record record)
        {
            return (record != null && record.State != RecordState.Normal) ?
                _error : null;
        }
    }
}
