using WebToTelegramCore.Models;

namespace WebToTelegramCore.Interfaces
{
    /// <summary>
    /// Interface that makes managing records look easy for its users.
    /// </summary>
    public interface IRecordService
    {
        /// <summary>
        /// Creates a new Record, setting common default values
        /// for all instances.
        /// </summary>
        /// <param name="token">Token to create Record with.</param>
        /// <param name="accountId">Account id to create token with.</param>
        /// <returns>Record with all properties populated.</returns>
        Record Create(string token, long accountId);

        /// <summary>
        /// Checks if Record holds enough charges to be able to send a message
        /// immediately (<see cref="Record.UsageCounter"/> > 0). If so, returns true,
        /// and updates Record's state to indicate the message was sent just now.
        /// </summary>
        /// <param name="record">Record to check and possibly update.</param>
        /// <returns>True if message can and should be sent, false otherwise.</returns>
        bool CheckIfCanSend(Record record);
    }
}
