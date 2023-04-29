using System;
using System.Reflection;
using WebToTelegramCore.Interfaces;
using WebToTelegramCore.Models;
using WebToTelegramCore.Resources;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that handles /about command that shows general info about the bot.
    /// </summary>
    public class AboutCommand : BotCommandBase, IBotCommand
    {
        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/about";

        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutCommand() : base() { }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Text of message that should be returned to user, with '.' escaped for MarkdownV2</returns>
        public override string Process(Record record)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            // imagine having to escape dot for "markdown"
            var prettyVersion = $"{version.Major}\\.{version.Minor}";
            return base.Process(record) ?? String.Format(Locale.About, prettyVersion);
        }
    }
}
