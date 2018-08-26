using WebToTelegramCore.Data;
using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Represents command /regenerate that marks user account to regenerate token.
    /// The command must be either confirmed or cancelled before using any other command.
    /// </summary>
    public class RegenerateCommand : UserOnlyCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/regenerate";

        // TODO: move to config
        /// <summary>
        /// Message that confirms regeneration is now pending.
        /// </summary>
        private const string _message = "*Token regenration pending!*\n\nPlease /confirm " +
            "or /cancel it. It cannot be undone. Please be certain.";

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
            record.State = RecordState.PendingRegeneration;
            return _message;
        }
    }
}
