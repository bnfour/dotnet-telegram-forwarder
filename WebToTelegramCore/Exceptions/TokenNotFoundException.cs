using System;

namespace WebToTelegramCore.Exceptions
{
    /// <summary>
    /// Exception that is thrown by <see cref="Services.OwnApiService"/>
    /// when user-supplied token does not match any registered in the database.
    /// Handled in the <see cref="Controllers.WebApiController"/>
    /// </summary>
    public class TokenNotFoundException : Exception { }
}
