using System;
using WebToTelegramCore.Helpers;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

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
        private readonly string[] _textExamples = new[]
        {
            "Hello world!",
            "Timeline lost",
            "send help",
            "inhale",
            "KNCA KYKY",
            "Hey Red",
            "Powered by .NET!",
            "robots can digest anything",
            "Are you still there?",
            "Could you come over here?",
            "Is it banana time yet?",
            "Try again later",
            "More than two and less than four",
            "Of course I still love you",
            "それは何?",
            "There was nothing to be sad about",
            "I never asked for this",
            "Everything in my life can unironically be solved with a full combo",
            "Synchronization failed",
            "Friends are so nice! heart"
        };

        /// <summary>
        /// Random sticker IDs to display as message example.
        /// </summary>
        private readonly string[] _stickerExamples = new[]
        {
            "CAACAgEAAxkBAAIBOGRXiFSUVC7ZsS_5q2TGB1-fpJhTAAIsEQACmX-IAgyEv_-IJmTBLwQ",
            "CAACAgQAAxkBAAIBOmRXiFra2cpvwzCWA0QYoVkmSwTlAAKiAgACZsMpDOGLaU-TMrXqLwQ",
            "CAACAgQAAxkBAAIBPGRXiGKI9bvYdPYccZJ2b0JZ-MrJAALLAANGp6MbBxkKM3Lb7egvBA",
        };

        /// <summary>
        /// Field to store API endpoint address.
        /// </summary>
        private readonly string _apiEndpoint;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="apiEndpoint">API endpoint URL.</param>
        public TokenCommand(string apiEndpoint) : base()
        {
            _apiEndpoint = apiEndpoint;
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
            var random = new Random();
            var text = _textExamples[random.Next(0, _textExamples.Length)];
            var sticker = _stickerExamples[random.Next(0, _stickerExamples.Length)];
            
            return String.Format(Locale.TokenTemplate, TelegramMarkdownFormatter.Escape(record.Token),
                TelegramMarkdownFormatter.Escape(_apiEndpoint + "/api"), TelegramMarkdownFormatter.Escape(text),
                TelegramMarkdownFormatter.Escape(sticker));
        }
    }
}
