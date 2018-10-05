using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WebToTelegramCore.FormatterHelpers
{
    /// <summary>
    /// Class that converts Telegram's flavor of Markdown to CommonMark
    /// understood by Markdig.
    /// </summary>
    public class MarkdownTransformer
    {
        /// <summary>
        /// List of sequences to escape. Contains single asterisk and underscore,
        /// as these are not formatting and should be left as-is.
        /// </summary>
        private static readonly List<string> _toEscape = new List<string>()
        {
            // * in regexes should be escaped
            @"\*",
            "_"
        };

        /// <summary>
        /// List of regexes that will escape symbols from _toEscape.
        /// </summary>
        private readonly List<Regex> _regexes = new List<Regex>();

        /// <summary>
        /// Constructor that sets up used regexes.
        /// </summary>
        public MarkdownTransformer()
        {
            foreach (var p in _toEscape)
            {
                var r = new Regex($@"([^{p}])({p})([^{p}])");
                _regexes.Add(r);
            }
        }

        /// <summary>
        /// Method to convert Telegram's markdown to CommonMark.
        /// </summary>
        /// <param name="TgMarkdown">String to convert.</param>
        /// <returns>String with converted formatting.</returns>
        public string ToCommonMark(string TgMarkdown)
        {
            string result = TgMarkdown;
            // escape single "*" and "_"
            foreach (var regex in _regexes)
            {
                result = regex.Replace(result, @"$1\$2$3");
            }

            result = result
                // fix italics
                .Replace("__", "*")
                // ensure triple backticks are on their own line to prevent misparsing
                .Replace("```", "\n```\n");

            return result;
        }
    }
}
