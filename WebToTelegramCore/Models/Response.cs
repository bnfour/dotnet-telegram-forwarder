namespace WebToTelegramCore.Models
{
    /// <summary>
    /// Represents web API response to user.
    /// Also in JSON, just like the initial request.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Quick indicator of whether request was accepted.
        /// </summary>
        public bool Ok { get; set; }

        /// <summary>
        /// Machine-readable error code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Human-readable description of error.
        /// </summary>
        public string Details { get; set; }
    }
}
