using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using WebToTelegramCore.BotCommands;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that implements handling webhook updates.
    /// </summary>
    public class TelegramApiService : ITelegramApiService
    {
        /// <summary>
        /// Bot's token. Used to verify update origin.
        /// </summary>
        private readonly string _token;

        /// <summary>
        /// Database context to use.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Bot service to send messages (and sometimes stickers).
        /// </summary>
        private readonly ITelegramBotService _bot;

        /// <summary>
        /// List of commands available to the bot.
        /// </summary>
        private readonly List<IBotCommand> _commands;

        /// <summary>
        /// Indicates whether usage of /create command is enabled.
        /// </summary>
        private bool _isRegistrationOpen;

        /// <summary>
        /// Constructor that injects dependencies and configures list of commands.
        /// </summary>
        /// <param name="options">Options that include token.</param>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service instance to use.</param>
        public TelegramApiService(IOptions<CommonOptions> options, RecordContext context,
            ITelegramBotService bot)
        {
            _token = options.Value.Token;
            _context = context;
            _bot = bot;

            _isRegistrationOpen = options.Value.RegistrationEnabled;

            _commands = new List<IBotCommand>()
            {
                // TODO actual commands
            };
        }

        /// <summary>
        /// Method to handle incoming updates from the webhook.
        /// </summary>
        /// <param name="update">Received update.</param>
        public void HandleUpdate(Update update)
        {
            // a few sanity checks
            if (update.Message.Type != MessageType.Text)
            {
                return;
            }

            long? userId = update?.Message?.From?.Id;
            string text = update?.Message?.Text;

            if (userId == null || String.IsNullOrEmpty(text))
            {
                return;
            }
            // null check was done above, it's safe to use userId.Value directly
            Record record = GetRecordByAccountId(userId.Value);

            IBotCommand handler = null;
            foreach (var command in _commands)
            {
                if (text.StartsWith(command.Command))
                {
                    handler = command;
                    break;
                }
            }

            if (handler != null)
            {
                _bot.Send(userId.Value, handler.Process(record));
            }
            else
            {
                HandleUnknownText(userId.Value, text);
            }
        }

        /// <summary>
        /// Checks whether passed string is an actual bot token to verify the request
        /// actully comes from Telegram backend.
        /// </summary>
        /// <param name="calledToken">Token received from request.</param>
        /// <returns>True if calledToken is actual token, false otherwise.</returns>
        public bool IsToken(string calledToken)
        {
            return calledToken.Equals(_token);
        }

        // TODO: move GetRecord* methods to context itself
        /// <summary>
        /// Gets Record associated with a given Telegram ID from the context.
        /// </summary>
        /// <param name="accountId">ID to search for.</param>
        /// <returns>Associated Record or null if none present.</returns>
        private Record GetRecordByAccountId(long accountId)
        {
            Record ret;
            try
            {
                ret = _context.Records.Single(r => r.AccountNumber == accountId);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            return ret;
        }

        /// <summary>
        /// Handles unknown text sent to bot.
        /// 5% chance of cat sticker, regular text otherwise.
        /// </summary>
        /// <param name="accountId">User to reply to.</param>
        /// <param name="text">Received message that was not processed
        /// by actual commands.</param>
        private void HandleUnknownText(long accountId, string text)
        {
            // suddenly, cat!
            if (new Random().Next(0, 19) == 0)
            {
                _bot.SendTheSticker(accountId);
            }
            else
            {
                string reply;
                // if it looks like a command, pretend we're HAL 9000
                // TODO: move these strings to config
                if (text.StartsWith("/"))
                {
                    reply = "I'm afraid I can't let you do that.";
                }
                else
                {
                    reply = "Unfortunately, I'm not sure how to interpret this. 🤔";
                }
                _bot.Send(accountId, reply);
            }
        }
    }
}
