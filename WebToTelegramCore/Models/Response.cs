using System.Collections.Generic;

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

        // TODO: make these localization-friendly (via appsettings.json, I guess?)

        /// <summary>
        /// Holds string representation of errors.
        /// </summary>
        private static Dictionary<ResponseState, string> _details
            = new Dictionary<ResponseState, string>()
        {
            [ResponseState.OkSent] = "Request accepted",
            [ResponseState.NoSuchToken] = "Invalid token",
            [ResponseState.BandwidthExceeded] = "Too many messages. Try again later",
            [ResponseState.SomethingBadHappened] = "Internal server error"
        };

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="state">ResponseState field values are based on.</param>
        public Response(ResponseState state)
        {
            Ok = state == ResponseState.OkSent;
            Code = (int)state;
            Details = _details[state];
        }
    }
}
