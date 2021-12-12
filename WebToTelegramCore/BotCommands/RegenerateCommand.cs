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
        /// Message that confirms regeneration is now pending.
        /// </summary>
        private readonly string _message;

        /// <summary>
        /// Constructor that sets up message.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        public RegenerateCommand(LocalizationOptions locale) : base(locale)
        {
            _message = locale.RegenerationPending;
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
            record.State = RecordState.PendingRegeneration;
            return _message;
        }
    }
}
