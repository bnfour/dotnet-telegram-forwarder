﻿using System;
using System.Reflection;
using WebToTelegramCore.Interfaces;
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
        /// Written in Telegram's MarkdownV2 with a lot of escaping.
        /// </summary>
        private const string _template = "**Dotnet Telegram forwarder** v {0}\n\n" +
            "[Open\\-source\\!](https://github.com/bnfour/dotnet-telegram-forwarder)\n" +
            "by bnfour, 2018, 2020\\-2023\\.";

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
        /// <param name="record">Record associated with user who sent the command.
        /// Unused here.</param>
        /// <returns>Text of message that should be returned to user, with '.' escaped for MarkdownV2</returns>
        public override string Process(Record record)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var prettyVersion = $"{version.Major}\\.{version.Minor}";
            return base.Process(record) ?? String.Format(_template, prettyVersion);
        }
    }
}
