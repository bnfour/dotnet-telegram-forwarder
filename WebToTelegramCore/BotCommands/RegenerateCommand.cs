using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

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

        /// <summary>
        /// Constructor.
        /// </summary>
        public RegenerateCommand() : base() { }

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
            return LocalizationOptions.RegenerationPending;
        }
    }
}
