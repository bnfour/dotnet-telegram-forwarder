using System;
using System.Reflection;

using WebToTelegramCore.Models;
using WebToTelegramCore.Options;

namespace WebToTelegramCore.BotCommands
{
    /// <summary>
    /// Class that handles /about command that shows general info about the bot.
    /// </summary>
    public class AboutCommand : BotCommandBase, IBotCommand
    {
        /// <summary>
        /// Template to message, {0} is assembly version.
        /// </summary>
        private const string _template = "**Dotnet Telegram forwarder** v. {0}\n\n" +
            "[Open-source!](https://github.com/bnfour/dotnet-telegram-forwarder) " +
            "Powered by ASP.NET Core!\n" +
            "Written by bnfour, August, October 2018; May 2020.\n\nN<3";

        /// <summary>
        /// Command's text.
        /// </summary>
        public override string Command => "/about";

        /// <summary>
        /// Constructor that passed localization options to base class.
        /// </summary>
        /// <param name="locale">Localization options to use.</param>
        public AboutCommand(LocalizationOptions locale) : base(locale) { }

        /// <summary>
        /// Method to process the command.
        /// </summary>
        /// <param name="userId">Telegram ID of the user who sent the command.
        /// Unused here.</param>
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Text of message that should be returned to user.</returns>
        public override string Process(long userId, Record record)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return base.Process(userId, record) ?? String.Format(_template, version);
        }
    }
}
