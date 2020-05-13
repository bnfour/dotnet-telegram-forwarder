using WebToTelegramCore.Models;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Interface to implement various bot commands without arguments.
    /// Used as text-only command interface.
    /// </summary>
    public interface IBotCommand
    {
        /// <summary>
        /// Text that should be used to invoke the command.
        /// Leading forward slash should be included here.
        /// </summary>
        string Command { get; }

        /// <summary>
        /// Method to process received message.
        /// </summary>
        /// <param name="userId">Telegram user ID, present even when no record is provided.
        /// Used mostly for <see cref="CreateCommand"/> (and any potential <see cref="GuestOnlyCommandBase"/> descendant)
        /// so extra processing can be removed.</param>
        /// <param name="record">Record associated with user who sent the command,
        /// or null if user has no Record (have not received the token).</param>
        /// <returns>Message to send back to user. Markdown is supported.</returns>
        string Process(long userId, Record record);
    }
}
