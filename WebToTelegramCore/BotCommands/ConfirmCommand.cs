using System;
using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that implements /confirm command which either deletes user's token or
    /// replaces it with a new one after a request via previous command.
    /// </summary>
    public class ConfirmCommand : ConfirmationCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/confirm";

        /// <summary>
        /// Database context reference to perform DB operations.
        /// </summary>
        private readonly RecordContext _context;

        /// <summary>
        /// Token generator service reference.
        /// </summary>
        private readonly ITokenGeneratorService _tokenGenerator;

        /// <summary>
        /// Record manipulation service helper reference.
        /// </summary>
        private readonly IRecordService _recordService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">Database context to use.</param>
        /// <param name="generator">Token generator to use.</param>
        /// <param name="recordService">Record helper to use.</param>
        public ConfirmCommand(RecordContext context, ITokenGeneratorService generator,
            IRecordService recordService) : base()
        {
            _context = context;
            _tokenGenerator = generator;
            _recordService = recordService;
        }

        /// <summary>
        /// Method to carry on confirmed destructive operations.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.</param>
        /// <returns>End-user readable result of the operation.</returns>
        public override string Process(Record record)
        {
            string baseResult = base.Process(record);
            if (baseResult != null)
            {
                return baseResult;
            }

            if (record.State == RecordState.PendingDeletion)
            {
                return Delete(record);
            }
            else
            {
                return Regenerate(record);
            }
        }

        /// <summary>
        /// Regenerates user's token.
        /// </summary>
        /// <param name="record">Record to generate new token for.</param>
        /// <returns>Message with new token.</returns>
        private string Regenerate(Record record)
        {
            string newToken = _tokenGenerator.Generate();
            // so apparently, primary key cannot be changed
            var newRecord = _recordService.Create(newToken, record.AccountNumber);
            _context.Remove(record);
            _context.Add(newRecord);
            _context.SaveChanges();
            return String.Format(LocalizationOptions.ConfirmRegeneration, newToken);
        }

        /// <summary>
        /// Deletes specified record from the database.
        /// </summary>
        /// <param name="record">Record to remove.</param>
        /// <returns>Message about performed operation.</returns>
        private string Delete(Record record)
        {
            _context.Remove(record);
            _context.SaveChanges();
            return LocalizationOptions.ConfirmDeletion;
        }
    }
}
