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
        /// Message to send. Max size is hardcoded to 4096 chars,
        /// same as Telegram's maximum length of a single message.
        /// </summary>
        [Required, StringLength(4096)]
        public string Message { get; set; }

        public bool Silent { get; set; } = false;
    }
}
