using WebToTelegramCore.Data;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /cancel command.
    /// </summary>
    public class CancelCommand : ConfirmationCommandBase, IBotCommand
    {
        // TODO: move to config
        /// <summary>
        /// Reply text when deletion was cancelled.
        /// </summary>
        private const string _deletionCancel = "Token deletion cancelled.";

        /// <summary>
        /// Reply text when regeneration was cancelled.
        /// </summary>
        private const string _regenerationCancel = "Token regeneration cancelled.";

        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/cancel";

        /// <summary>
        /// Method to process the command. Resets Record's State back to Normal.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(Record record)
        {
            string baseResult = base.Process(record);
            if (baseResult != null)
            {
                return baseResult;
            }

            string reply = record.State == RecordState.PendingDeletion ?
                _deletionCancel : _regenerationCancel;

            record.State = RecordState.Normal;
            return reply;
        }
    }
}
