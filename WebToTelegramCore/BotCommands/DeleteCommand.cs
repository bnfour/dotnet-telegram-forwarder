using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
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
        /// Boolean that indicates whether this instance is accepting new users.
        /// True if it does.
        /// </summary>
        private readonly bool _registrationEnabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="registrationEnabled">Registration state. True is enabled.</param>
        public DeleteCommand(bool registrationEnabled) : base()
        {
            _registrationEnabled = registrationEnabled;
        }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Message with results of processing.</returns>
        public override string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Actual method to update record's state.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Message confirming that deletion is now pending.</returns>
        private string InternalProcess(Record record)
        {
            record.State = RecordState.PendingDeletion;
            return _registrationEnabled
                ? LocalizationOptions.DeletionPending
                : LocalizationOptions.DeletionPending + "\n\n" + LocalizationOptions.DeletionNoTurningBack;
        }
    }
}
