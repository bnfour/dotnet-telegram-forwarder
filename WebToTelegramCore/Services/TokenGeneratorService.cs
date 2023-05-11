using System;
using System.Security.Cryptography;
using System.Linq;
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
        /// DB context. Used to check for collisions with existing tokens.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public TokenGeneratorService(RecordContext context)
        {
            _context = context;

            // sanity check for random evenness
            // TODO consider it to be a warning instead of an exception
            if (256 % _alphabet.Length != 0)
            {
                throw new ApplicationException("Selected alphabet does not map evenly " +
                    "to bytes value. Consider alphabet length that is a divider of 256.");
            }
        }

        /// <summary>
        /// Generates a token and ensures it is not yet assigned to other accounts.
        /// </summary>
        /// <returns>An unique token.</returns>
        public string Generate()
        {
            string token = null;
            var done = false;
            // let's pretend we're serious business just for a moment
            using (var random = RandomNumberGenerator.Create())
            {
                while (!done)
                {
                    token = GenerateRandom(random);
                    done = !_context.Records.Any(r => r.Token == token);
                }
            }
            return token;
        }

        /// <summary>
        /// Actual token generation method.
        /// </summary>
        /// <param name="random">A _secure_ random generator to use.</param>
        /// <returns>A completely random token.</returns>
        private string GenerateRandom(RandomNumberGenerator random)
        {
            var randomBytes = new byte[_tokenLength];
            random.GetBytes(randomBytes);

            var sb = new StringBuilder();
            foreach (var b in randomBytes)
            {
                sb.Append(_alphabet[b % _alphabet.Length]);
            }

            return sb.ToString();
        }
    }
}
