using System.ComponentModel.DataAnnotations;

namespace WebToTelegramCore.Models
{
    /// <summary>
    /// Represents user's request to our web API.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Auth token.
        /// </summary>
        [Required, RegularExpression(@"[a-zA-Z0-9+=]{16}")]
        public string Token { get; set; }

        /// <summary>
        /// Message to send. Max size is hardcoded to 8192 chars.
        /// </summary>
        [Required, StringLength(8192)]
        public string Message { get; set; }
    }
}
