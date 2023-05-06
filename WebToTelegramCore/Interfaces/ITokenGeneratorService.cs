namespace WebToTelegramCore.Interfaces
{
    /// <summary>
    /// Interface to services that generate auth tokens.
    /// </summary>
    public interface ITokenGeneratorService
    {
        // Tokens are strings of length 16 which contain letters a to z, both
        // uppercase and lowercase, digits zero to nine and also + and = symbols
        // for a total of 64

        /// <summary>
        /// Generates a new token.
        /// </summary>
        /// <returns>Generated token as a string.</returns>
        string Generate();
    }
}
