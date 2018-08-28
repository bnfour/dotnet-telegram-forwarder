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
        /// Reference to token generator service.
        /// </summary>
        private readonly ITokenGeneratorService _generator;

        /// <summary>
        /// List of commands available to the bot.
        /// </summary>
        private readonly List<IBotCommand> _commands;

        /// <summary>
        /// /create handler, as it requires special treating since i'm bad at programming.
        /// </summary>
        private readonly CreateCommand _thatOneCommand;

        /// <summary>
        /// Indicates whether usage of /create command is enabled.
        /// </summary>
        private readonly bool _isRegistrationOpen;

        /// <summary>
        /// Message to reply with when input is starting with slash, but none of the
        /// commands fired in response.
        /// </summary>
        private readonly string _invalidCommandReply;

        /// <summary>
        /// Message to reply with when input isn't even resembles a command.
        /// </summary>
        private readonly string _invalidReply;

        /// <summary>
        /// Constructor that injects dependencies and configures list of commands.
        /// </summary>
        /// <param name="options">Options that include token.</param>
        /// <param name="locale">Localization options.</param>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service instance to use.</param>
        /// <param name="generator">Token generator service to use.</param>
        public TelegramApiService(IOptions<CommonOptions> options, 
            IOptions<LocalizationOptions> locale, RecordContext context,
            ITelegramBotService bot, ITokenGeneratorService generator)
        {
            _token = options.Value.Token;
            _context = context;
            _bot = bot;
            _generator = generator;

            _isRegistrationOpen = options.Value.RegistrationEnabled;

            LocalizationOptions locOptions = locale.Value;

            _invalidCommandReply = locOptions.ErrorDave;
            _invalidReply = locOptions.ErrorWhat;

            _commands = new List<IBotCommand>()
            {
                new StartCommand(locOptions, _isRegistrationOpen),
                new TokenCommand(locOptions, options.Value.ApiEndpointUrl),
                new RegenerateCommand(locOptions),
                new DeleteCommand(locOptions, _isRegistrationOpen),
                new ConfirmCommand(locOptions, _context, _generator),
                new CancelCommand(locOptions),
                new HelpCommand(locOptions),
                new DirectiveCommand(locOptions),
                new AboutCommand(locOptions)

            };
            _thatOneCommand = new CreateCommand(locOptions, _context, _generator,
                _isRegistrationOpen);
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
            Record record = _context.GetRecordByAccountId(userId.Value);

            IBotCommand handler = null;
            if (text.StartsWith(_thatOneCommand.Command))
            {
                handler = _thatOneCommand;
                _thatOneCommand.Crutch = userId.Value;
            }
            else
            {
                _thatOneCommand.Crutch = null;
                handler = _commands.SingleOrDefault(c => text.StartsWith(c.Command));
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
                string reply = text.StartsWith("/") ?
                    _invalidCommandReply : _invalidReply;
                _bot.Send(accountId, reply);
            }
        }
    }
}
