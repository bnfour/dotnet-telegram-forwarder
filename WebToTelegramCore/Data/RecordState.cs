namespace WebToTelegramCore.Data
{
    /// <summary>
    /// Represents states a Record can be in. Used to keep track and to confirm
    /// destructive operations -- token deletion or regenration.
    /// </summary>
    public enum RecordState
    {
        /// <summary>
        /// Normal state, all bot commands except '/cancel' and '/confirm' are available.
        /// </summary>
        Normal,
        /// <summary>
        /// Only valid bot commands are '/confirm' to delete token
        /// and '/cancel' to set state back to normal.
        /// </summary>
        PendingDeletion,
        /// <summary>
        /// Only valid commands are '/confirm' to regenrate token
        /// and '/cancel' to set state back to normal.
        /// </summary>
        PendingRegeneration
    }
}
