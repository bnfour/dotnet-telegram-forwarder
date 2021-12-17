using WebToTelegramCore.Models;

namespace WebToTelegramCore.Interfaces
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
        /// <param name="record">Record associated with user who sent the command,
        /// or a mock Record with everything but account id set to default values.</param>
        /// <returns>Message to send back to user. Markdown is supported.</returns>
        string Process(Record record);
    }
}
