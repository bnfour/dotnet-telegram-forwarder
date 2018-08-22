namespace WebToTelegramCore.Options
{
    /// <summary>
    /// Class that represents app's setting related to message
    /// throughput limitations.
    /// </summary>
    public class BandwidthOptions
    {
        /// <summary>
        /// Represents initial number of messages available.
        /// </summary>
        public int InitialCount { get; set; }

        /// <summary>
        /// Represents amount of seconds after last message send to regenerate
        /// one message.
        /// </summary>
        public int SecondsPerRegeneration { get; set; }
    }
}
