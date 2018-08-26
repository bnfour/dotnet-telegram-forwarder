using WebToTelegramCore.Data;
using WebToTelegramCore.Models;

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

        // TODO: move to config
        /// <summary>
        /// Message that confirms deletion is now pending.
        /// </summary>
        private const string _message = "*Token deletion pending!*\n\nPlease /confirm " +
            "or /cancel it. It cannot be undone.\nIf you need to change your token, " +
            "please consider to /regenerate it instead of deleting and recreating it.";

        /// <summary>
        /// Additional warning shown when registration is turned off.
        /// </summary>
        private const string _noTurningBack = "This bot has registration turned *off*. " +
            "You won't be able to create new token. Please be certain.";

        /// <summary>
        /// Boolean that indicates whether this instance is accepting new users.
        /// True if it does.
        /// </summary>
        private readonly bool _registrationEnabled;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="registrationEnabled">Registration state. True is enabled.</param>
        public DeleteCommand(bool registrationEnabled)
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
            return _registrationEnabled ? _message : _message + "\n\n" + _noTurningBack;
        }
    }
}
