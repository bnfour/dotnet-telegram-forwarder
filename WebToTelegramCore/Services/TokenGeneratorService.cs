using System;
using System.Security.Cryptography;
using System.Text;

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
        /// Strong random instance used to generate random tokens.
        /// </summary>
        private readonly RandomNumberGenerator _random;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public TokenGeneratorService()
        {
            // let's pretend we're serious business for a moment
            _random = RandomNumberGenerator.Create();
            // sanity check for random evenness
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
            _random.GetBytes(randomBytes);

            var sb = new StringBuilder();
            foreach (var b in randomBytes)
            {
                sb.Append(_alphabet[b % _alphabet.Length]);
            }

            return sb.ToString();
        }
    }
}
