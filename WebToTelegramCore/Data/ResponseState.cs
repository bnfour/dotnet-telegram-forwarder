namespace WebToTelegramCore.Data
{
    /// <summary>
    /// Represents various web API calls outcomes.
    /// </summary>
    public enum ResponseState
    {
        /// <summary>
        /// Request accepted. No further action by client needed.
        /// </summary>
        OkSent,
        /// <summary>
        /// Token not found. Client should not retry.
        /// </summary>
        NoSuchToken,
        /// <summary>
        /// Too many messages at a time. Client should retry later.
        /// </summary>
        BandwidthExceeded,
        /// <summary>
        /// An exception occured. Client may or may not retry later.
        /// </summary>
        SomethingBadHappened
    }
}
