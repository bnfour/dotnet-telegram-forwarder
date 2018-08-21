using System;

namespace WebToTelegramCore.Models
{
    /// <summary>
    /// Internal representation of Token - ID relationship with additional
    /// fields to control usage.
    /// </summary>
    public class Record
    {
        // TODO: make this configurable, hence static, not const

        /// <summary>
        /// Maximum possible amount of messages available immidiately.
        /// </summary>
        private static int _counterMax = 20;

        /// <summary>
        /// Auth token associated with this record. Primary key in the DB.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// ID of connected Telegram account.
        /// </summary>
        public int AccountNumber { get; set; }

        /// <summary>
        /// Backing field of UsageCounter property.
        /// </summary>
        private int _counter;

        /// <summary>
        /// Holds amount of messages available immidiately, prevents over- and underflows.
        /// </summary>
        public int UsageCounter
        {
            get => _counter;
            set
            {
                if (value <= 0)
                {
                    _counter = 0;
                }
                else if (value >= _counterMax)
                {
                    _counter = _counterMax;
                }
                else
                {
                    _counter = value;
                }
            }
        }

        /// <summary>
        /// Timestamp of last successful request. Used to calculate how much to add
        /// to UsageCounter.
        /// </summary>
        public DateTime LastSuccessTimestamp { get; set; }

        /// <summary>
        /// Constructor that sets up default values for properties not stored in the DB.
        /// </summary>
        public Record()
        {
            _counter = _counterMax;
            LastSuccessTimestamp = DateTime.Now;
        }
    }
}
