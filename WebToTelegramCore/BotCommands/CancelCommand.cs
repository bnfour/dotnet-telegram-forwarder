using WebToTelegramCore.Data;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /cancel command.
    /// </summary>
    public class CancelCommand : ConfirmationCommandBase, IBotCommand
    {
        /// <summary>
        /// Reply text when deletion was cancelled.
        /// </summary>
        private readonly string _deletionCancel;

        /// <summary>
        /// Reply text when regeneration was cancelled.
        /// </summary>
        private readonly string _regenerationCancel;

        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/cancel";

        /// <summary>
        /// Constructor that sets error message.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public CancelCommand(LocalizationOptions locale) : base(locale)
        {
            _deletionCancel = locale.CancelDeletion;
            _regenerationCancel = locale.CancelRegeneration;
        }

        /// <summary>
        /// Method to process the command. Resets Record's State back to Normal.
        /// </summary>
        /// <param name="userId">Telegram user ID. Unused here.</param>
        /// <param name="record">Record associated with user who sent the command.</param>
        /// <returns>Predefined text if all checks from parent classes passed,
        /// corresponding error message otherwise.</returns>
        public override string Process(long userId, Record record)
        {
            string baseResult = base.Process(userId, record);
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
