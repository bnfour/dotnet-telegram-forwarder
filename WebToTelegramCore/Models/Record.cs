using System;
using WebToTelegramCore.Data;

namespace WebToTelegramCore.Models
{
    /// <summary>
    /// Internal representation of Token - ID relationship with additional
    /// fields to control usage.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Auth token associated with this record. Primary key in the DB.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// ID of connected Telegram account.
        /// </summary>
        public long AccountNumber { get; set; }

        /// <summary>
        /// Holds amount of messages available immidiately.
        /// </summary>
        public int UsageCounter { get; set; }

        /// <summary>
        /// Timestamp of last successful request. Used to calculate how much to add
        /// to UsageCounter.
        /// </summary>
        public DateTime LastSuccessTimestamp { get; set; }

        /// <summary>
        /// Field to store whether this Record awaits user confirmation
        /// on destructive command.
        /// </summary>
        public RecordState State { get; set; }
    }
}
