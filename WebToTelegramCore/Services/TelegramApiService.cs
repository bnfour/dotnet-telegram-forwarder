using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using WebToTelegramCore.BotCommands;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;
using WebToTelegramCore.Resources;

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
        /// Record manipulation service helper reference.
        /// </summary>
        private readonly IRecordService _recordService;

        /// <summary>
        /// List of commands available to the bot.
        /// </summary>
        private readonly List<IBotCommand> _commands;

        /// <summary>
        /// Constructor that injects dependencies and configures list of commands.
        /// </summary>
        /// <param name="options">Options that include token.</param>
        /// <param name="context">Database context to use.</param>
        /// <param name="bot">Bot service instance to use.</param>
        /// <param name="generator">Token generator service to use.</param>
        /// <param name="recordService">Record helper service to use.</param>
        public TelegramApiService(IOptions<CommonOptions> options,
            RecordContext context, ITelegramBotService bot,
            ITokenGeneratorService generator, IRecordService recordService)
        {
            _token = options.Value.Token;
            _context = context;
            _bot = bot;
            _generator = generator;
            _recordService = recordService;

            var isRegistrationOpen = options.Value.RegistrationEnabled;

            _commands = new List<IBotCommand>()
            {
                new StartCommand(isRegistrationOpen),
                new TokenCommand(options.Value.ApiEndpointUrl),
                new RegenerateCommand(),
                new DeleteCommand(isRegistrationOpen),
                new ConfirmCommand(_context, _generator, _recordService),
                new CancelCommand(),
                new HelpCommand(),
                new DirectiveCommand(),
                new AboutCommand(),
                new CreateCommand(_context, _generator, _recordService, isRegistrationOpen)
            };
        }

        /// <summary>
        /// Method to handle incoming updates from the webhook.
        /// </summary>
        /// <param name="update">Received update.</param>
        public async Task HandleUpdate(Update update)
        {
            // only handles either text messages, hopefully commands, or stickers
            switch (update.Message.Type)
            {
                case MessageType.Text:
                    await HandleTextMessage(update);
                break;
                case MessageType.Sticker:
                    await HandleSticker(update);
                break;
                default:
                    return;
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
        /// Method to handle incoming text updates from the webhook.
        /// </summary>
        /// <param name="update">Received update.</param>
        private async Task HandleTextMessage(Update update)
        {
            long? userId = update?.Message?.From?.Id;
            string text = update?.Message?.Text;
            // check if update contains everything we need to process it
            if (userId == null || string.IsNullOrEmpty(text))
            {
                return;
            }
            // if user has no record associated, make him a mock one with just an account number,
            // so we know who they are in case we're going to create them a proper one
            Record record = await _context.GetRecordByAccountId(userId.Value)
                ?? _recordService.Create(null, userId.Value);

            IBotCommand handler = null;
            string commandText = text.Split(' ').FirstOrDefault();
            // will crash if multiple command classes share same text, who cares
            handler = _commands.SingleOrDefault(c => c.Command.Equals(commandText));

            if (handler != null)
            {
                await _bot.Send(userId.Value, handler.Process(record));
            }
            else
            {
                await HandleUnknownText(userId.Value, commandText);
            }
        }

        /// <summary>
        /// Handles unknown text sent to bot.
        /// 5% chance of cat sticker, regular text otherwise.
        /// </summary>
        /// <param name="accountId">User to reply to.</param>
        /// <param name="text">Received message that was not processed
        /// by actual commands.</param>
        private async Task HandleUnknownText(long accountId, string text)
        {
            // suddenly, cat!
            if (new Random().Next(0, 19) == 0)
            {
                await _bot.SendTheSticker(accountId);
            }
            else
            {
                string reply = text.StartsWith("/")
                    ? Locale.ErrorDave
                    : Locale.ErrorWhat;
                await _bot.Send(accountId, reply);
            }
        }

        /// <summary>
        /// Method to handle incoming sticker updates from the webhook.
        /// Replies with an 
        /// </summary>
        /// <param name="update">Received update.</param>
        private async Task HandleSticker(Update update)
        {
            long? userId = update?.Message?.From?.Id;
            // TODO do we need FileId or FileUniqueId?
            string fileId = update?.Message.Sticker?.FileId;
            string fileUniqueId = update?.Message.Sticker?.FileUniqueId;
            // check if update contains everything we need to process it
            // TODO breakpoint to check ids
            if (userId == null
                || (string.IsNullOrEmpty(fileId) && string.IsNullOrEmpty(fileUniqueId)))
            {
                return;
            }
            // TODO format a template with a single id to send to the web API
            // (actually make a template, determine which id to use)
            await _bot.Send(userId.Value, "Okayeg eg", parsingType: Data.MessageParsingType.Plaintext);
        }
    }
}
