using System;

namespace WebToTelegramCore.Exceptions
{
    /// <summary>
    /// Exception that is thrown by <see cref="Services.OwnApiService"/>
    /// when used have exceeded their allowed bandwidth.
    /// Handled in the <see cref="Controllers.WebApiController"/>
    /// </summary>
    public class BandwidthExceededException : Exception { }
}
