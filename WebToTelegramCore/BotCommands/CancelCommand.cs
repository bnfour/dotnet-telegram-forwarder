using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /cancel command to cancel pending destructive
    /// operations.
    /// </summary>
    public class CancelCommand : ConfirmationCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/cancel";

        /// <summary>
        /// Constructor.
        /// </summary>
        public CancelCommand() : base() { }

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

            string reply = record.State == RecordState.PendingDeletion
                ? Locale.CancelDeletion
                : Locale.CancelRegeneration;

            record.State = RecordState.Normal;
            return reply;
        }
    }
}
