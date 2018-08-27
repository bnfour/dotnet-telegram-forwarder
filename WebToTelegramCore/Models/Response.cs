using ResponseState = WebToTelegramCore.Data.ResponseState;

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
        public bool Ok { get; private set; }

        /// <summary>
        /// Machine-readable error code.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Human-readable description of error.
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="state">ResponseState field values are based on.</param>
        /// <param name="details">Human-readable message corresponding to state.</param>
        public Response(ResponseState state, string details)
        {
            Ok = state == ResponseState.OkSent;
            Code = (int)state;
            Details = details;
        }
    }
}
