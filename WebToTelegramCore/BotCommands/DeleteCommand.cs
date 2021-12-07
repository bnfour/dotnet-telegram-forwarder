using WebToTelegramCore.Data;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Represents command /delete that marks user account to deletion than must be
    /// either confirmed or cancelled before using any other command.
    /// </summary>
    public class DeleteCommand : UserOnlyCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/delete";

        /// <summary>
        /// Message that confirms deletion is now pending.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Additional warning shown when registration is turned off.
        /// </summary>
        private readonly string _noTurningBack;

        /// <summary>
        /// Boolean that indicates whether this instance is accepting new users.
        /// True if it does.
        /// </summary>
        private readonly bool _registrationEnabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        /// <param name="registrationEnabled">Registration state. True is enabled.</param>
        public DeleteCommand(LocalizationOptions locale, bool registrationEnabled)
            : base(locale)
        {
            _registrationEnabled = registrationEnabled;

            _message = locale.DeletionPending;
            _noTurningBack = locale.DeletionNoTurningBack;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="userId">Unused Telegram user ID.</param>
        /// <param name="record">Record to process.</param>
        /// <returns>Message with results of processing.</returns>
        public override string Process(long userId, Record record)
        {
            return base.Process(userId, record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Actual method to update record's state.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Message confirming that deletion is now pending.</returns>
        private string InternalProcess(Record record)
        {
            record.State = RecordState.PendingDeletion;
            return _registrationEnabled ? _message : _message + "\n\n" + _noTurningBack;
        }
    }
}
