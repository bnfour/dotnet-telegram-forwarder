using System;
using System.Security.Cryptography;
using System.Text;
using WebToTelegramCore.Interfaces;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that actually generates tokens.
    /// </summary>
    internal class TokenGeneratorService : ITokenGeneratorService
    {
        /// <summary>
        /// Hardcoded token length.
        /// </summary>
        private const int _tokenLength = 16;

        /// <summary>
        /// Symbols that token may contain. Total count must be a divider of 256
        /// in order to random to be even.
        /// </summary>
        private static readonly char[] _alphabet = ("0123456789" + "+=" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz").ToCharArray();

        /// <summary>
        /// Class constructor.
        /// </summary>
        public TokenGeneratorService()
        {
            // sanity check for random evenness
            // TODO consider it to be a warning instead of an exception
            if (256 % _alphabet.Length != 0)
            {
                throw new ApplicationException("Selected alphabet does not map evenly " +
                    "to bytes value. Consider alphabet length that is a divider of 256.");
            }
        }

        /// <summary>
        /// Token generation method.
        /// </summary>
        /// <returns>Token.</returns>
        public string Generate()
        {
            var randomBytes = new byte[_tokenLength];
            // let's pretend we're serious business for a moment
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(randomBytes);
            }

            var sb = new StringBuilder();
            foreach (var b in randomBytes)
            {
                sb.Append(_alphabet[b % _alphabet.Length]);
            }

            return sb.ToString();
        }
    }
}
