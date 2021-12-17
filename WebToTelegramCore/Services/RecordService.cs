using Microsoft.Extensions.Options;
using System;
using WebToTelegramCore.Data;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.Services
{
    /// <summary>
    /// Class that makes managing Records look easy for its users.
    /// hide_the_pain_harold.jpg
    /// </summary>
    public class RecordService : IRecordService
    {
        /// <summary>
        /// Maximum possible amount of messages available immidiately.
        /// </summary>
        private readonly int _counterMax;

        /// <summary>
        /// Amount of seconds lince last successful API call to regenerate counter.
        /// </summary>
        private readonly TimeSpan _timeToRegen;

        /// <summary>
        /// Constructor that gets message bandwiths settings for later use.
        /// </summary>
        /// <param name="options">Options to use.</param>
        public RecordService(IOptions<BandwidthOptions> options)
        {
            _counterMax = options.Value.InitialCount;
            _timeToRegen = TimeSpan.FromSeconds(options.Value.SecondsPerRegeneration);
        }

        /// <summary>
        /// Creates a new Record, setting common default values
        /// for all instances.
        /// </summary>
        /// <param name="token">Token to create Record with.</param>
        /// <param name="accountId">Account id to create token with.</param>
        /// <returns>Record with all properties populated.</returns>
        public bool CheckIfCanSend(Record record)
        {
            var sinceLastSuccess = DateTime.UtcNow - record.LastSuccessTimestamp;
            var toAdd = (int)(sinceLastSuccess / _timeToRegen);

            record.UsageCounter = Math.Min(_counterMax, record.UsageCounter + toAdd);

            if (record.UsageCounter > 0)
            {
                record.LastSuccessTimestamp = DateTime.Now;
                record.UsageCounter--;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if Record holds enough charges to be able to send a message
        /// immediately (<see cref="Record.UsageCounter"/> > 0). If so, returns true,
        /// and updates Record's state to indicate the message was sent just now.
        /// </summary>
        /// <param name="record">Record to check and possibly update.</param>
        /// <returns>True if message can and should be sent, false otherwise.</returns>
        public Record Create(string token, long accountId)
        {
            return new Record
            {
                AccountNumber = accountId,
                LastSuccessTimestamp = DateTime.UtcNow,
                State = RecordState.Normal,
                Token = token,
                UsageCounter = _counterMax
            };
        }
    }
}
