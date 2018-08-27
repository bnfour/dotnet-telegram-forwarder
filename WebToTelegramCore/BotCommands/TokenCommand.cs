using System;
using System.Collections.Generic;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// /token command. Displays user's token and usage hint.
    /// </summary>
    public class TokenCommand : UserOnlyCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/token";

        /// <summary>
        /// Random quotes to display as message example.
        /// </summary>
        private readonly List<string> _examples = new List<string>()
        {
            "Hello world!",
            "Timeline lost",
            "send help",
            "`inhale`",
            "KNCA KYKY",
            "Hey Red",
            "Powered by .NET!",
            "robots can digest anything",
            "Are you still there?",
            "Could you come over here?",
            "Is it banana time yet?",
            "Try again later",
            "More than two and less than four",
            "Of course I still love you"
        };

        /// <summary>
        /// Field to store API endpoint address.
        /// </summary>
        private readonly string _apiEndpoint;

        /// <summary>
        /// Template for reply with three formatters: {0} is token, {1} is API endpoint,
        /// {2} is random vanity message example.
        /// </summary>
        private readonly string _templateOne;

        /// <summary>
        /// Message explaining Response structure and roles of its fields.
        /// </summary>
        private readonly string _errors;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="locale">Locale options to use.</param>
        /// <param name="apiEndpoint">API endpoint URL.</param>
        public TokenCommand(LocalizationOptions locale, string apiEndpoint)
            : base(locale)
        {
            _apiEndpoint = apiEndpoint;

            _templateOne = locale.TokenTemplate;
            _errors = locale.TokenErrorsDescription;
        }

        /// <summary>
        /// Method to process command.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.</param>
        /// <returns>Message with token and API usage, or error message if user
        /// has no token.</returns>
        public override string Process(Record record)
        {
            return base.Process(record) ?? InternalProcess(record);
        }

        /// <summary>
        /// Actual method to create returned message with placeholders filled.
        /// </summary>
        /// <param name="record">Record to process.</param>
        /// <returns>Message with token and API usage example.</returns>
        private string InternalProcess(Record record)
        {
            string text = _examples[new Random().Next(0, _examples.Count)];
            return String.Format(_templateOne, record.Token, _apiEndpoint, text)
                + "\n\n" + _errors;
        }
    }
}
