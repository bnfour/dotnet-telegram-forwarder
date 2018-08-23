﻿using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Base class for commands that are both not confirmation commands and
    /// available only to users which already have a token.
    /// </summary>
    public abstract class UserOnlyCommandBase : BotCommandBase, IBotCommand
    {
        // TODO: move to config
        /// <summary>
        /// Text somewhat explaining why processing of this Record
        /// was cancelled in this class.
        /// </summary>
        private const string _error = "In order to use this command, you must have " +
            "a token associated with your account. Try running `/create` first.";

        /// <summary>
        /// Method of abstract base class that adds filtering out users
        /// with no associated token.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if there is an operation pending or user has no token,
        /// or null otherwise.</returns>
        public new virtual string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Method that filters out users with no tokens.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Error message if user has no token,
        /// or null otherwise.</returns>
        private string InternalProcess(Record record)
        {
            return record == null ? _error : null;
        }
    }
}
