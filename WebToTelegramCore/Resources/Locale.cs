namespace WebToTelegramCore.Resources
{
    /// <summary>
    /// Class that holds all customizable string the bot may reply with.
    /// Also, descriptions of error codes.
    /// Not ideal, but slightly better (is it?) than holding these in appsettings.json.
    /// </summary>

    // Please note that these should be formatted as Telegram-flavoured Markdown
    // see https://core.telegram.org/bots/api#markdownv2-style
    // Templating parameters (like {0}) are filled in before sending to the API,
    // so { and } in these should NOT be escaped. But the actual text in these should be.

    public static class Locale
    {
        /// <summary>
        /// Template for reply for /about command. {0} is assembly version.
        /// </summary>
        public const string About = """
        **Dotnet Telegram forwarder** v {0}\.

        [Open\-source\!](https://github.com/bnfour/dotnet-telegram-forwarder)
        by bnfour, 2018, 2020\-2023\.
        """;

        /// <summary>
        /// Message to show when token deletion was cancelled.
        /// </summary>
        public const string CancelDeletion = """
        Token deletion cancelled\.
        """;
        
        /// <summary>
        /// Message to show when token regeneration was cancelled.
        /// </summary>
        public const string CancelRegeneration = """
        Token regeneration cancelled\.
        """;

        /// <summary>
        /// Message to show when token deletion was completed.
        /// </summary>
        public const string ConfirmDeletion = """
        Token deleted\.
        Thank you for giving us an oppurtunity to serve you\.
        """;

        /// <summary>
        /// Template for message to show when token regeneration was completed.
        /// {0} is new token.
        /// </summary>
        public const string ConfirmRegeneration = """
        Your new token is

        `{0}`

        Don't forget to update your clients' settings\.
        """;
        
        /// <summary>
        /// Message to reply to /create with when registration is disabled.
        /// </summary>
        public const string CreateGoAway = """
        This instance of the bot is not accepting new users for now\.
        """;
        
        /// <summary>
        /// Template for message to show when token was created successfully.
        /// {0} is new token.
        /// </summary>
        public const string CreateSuccess = """
        Success\! Your token is:

        `{0}`

        Please consult /token for usage\.
        """;
        
        /// <summary>
        /// Message to show when user initiated token deletion and registration is off.
        /// </summary>
        public const string DeletionNoTurningBack = """
        This bot has registration turned *off*\. You won't be able to create new token\. Please be certain\.
        """;

        /// <summary>
        /// Message to show when user initiated token deletion.
        /// </summary>
        public const string DeletionPending = """
        *Token deletion pending\!*

        Please either /confirm or /cancel it\. This cannot be undone\.
        If you need to change your token, consider to /regenerate it instead of deleting and re\-creating\.
        """;
        
        /// <summary>
        /// Message to show when confirmation is pending and user tries to use
        /// non-confirming command.
        /// </summary>
        public const string ErrorConfirmationPending = """
        You have an operation pending confirmation\. Please /confirm or /cancel it before using other commands\.
        """;
        
        /// <summary>
        /// Message to show to unknown commands starting with slash.
        /// </summary>
        public const string ErrorDave = """
        I'm afraid I can't let you do that\.
        """;
        
        /// <summary>
        /// Message to show when usage of command requires no token set,
        /// but user has one.
        /// </summary>
        public const string ErrorMustBeGuest = """
        In order to use this command, you must have no token associated with your account\. You can /delete your existing one, but why?
        """;
        
        /// <summary>
        /// Message to show when usage of command requires a token, but user has none.
        /// </summary>
        public const string ErrorMustBeUser = """
        In order to use this command, you must have a token associated with your account\. /create one\.
        """;
        
        /// <summary>
        /// Message to show when user is trying to use confirming command, but no
        /// confirmation is pending.
        /// </summary>
        public const string ErrorNoConfirmationPending = """
        This command is only useful when you're trying to /delete or /regenerate your token\.
        """;
        
        /// <summary>
        /// Message to show on unknown input.
        /// </summary>
        public const string ErrorWhat = """
        Unfortunately, I'm not sure how to interpret this\.
        """;
        
        /// <summary>
        /// Output of /help command.
        /// </summary>
        public const string Help = """
        This app provides a web API to route messages from API's endpoint to you in Telegram via this bot\. It can be used to notify you in Telegram from any Internet\-enabled device you want \(provided you know how to make POST requests from it\)\.
        To start, /create your token\. You can /delete it anytime\. To change your token for whatever reason, please /regenerate it and not delete and re\-create\.
        Once you have a token, see /token for additional usage help\.

        Other commands supported by bot include:
        \- /confirm and /cancel are used to prevent accidental deletions and regenerations;
        \- /help displays this message;
        \- /about displays general info about this bot;
        \- Send any sticker to get its identifier\.

        \-\-bnfour
        """;
        
        /// <summary>
        /// Message to show when user initiated token regeneration.
        /// </summary>
        public const string RegenerationPending = """
        *Token regenration pending\!*

        Please either /confirm or /cancel it\. It cannot be undone\. Please be certain\.
        """;
        
        /// <summary>
        /// Message to warn new users that registration is closed.
        /// </summary>
        public const string StartGoAway = """
        Sorry, this instance of bot is not accepting new users for now\.
        """;
        
        /// <summary>
        /// Message to show when user first engages the bot.
        /// </summary>
        public const string StartMessage = """
        Hello\!

        This bot provides a standalone web API to relay messages \(text or stickers\) from anything that can send web requests to your Telegram inbox as messages from the bot\.
        
        *Please note*: this requires some external tools and knowledge\. If you consider "Send a POST request" a magic gibberish you don't understand, this bot probably isn't much of use to you\.
        """;
        
        /// <summary>
        /// Message to encourage new users to register.
        /// </summary>
        public const string StartRegistrationHint = """
        If that does not stop you, feel free to /create your very own token\.
        """;

        /// <summary>
        /// Reply to a sticker message with its ID to use with the web API.
        /// {0} is sticker's FileId
        /// </summary>
        public const string StickerId = """
        This sticker's FileId is

        `{0}`
        """;

        /// <summary>
        /// Template for message to reply to /token command with.
        /// {0} is token, {1} is API endpoint URL, {2} is vanity quote,
        /// {3} is vanity sticker id.
        /// </summary>
        // double braces are escaping for formatting
        public const string TokenTemplate = """
        Your token is

        `{0}`

        *Usage:* To deliver a message, send a POST request to {1} with JSON body\. The payload must contain two parameters: the token and your message or sticker id\. See [the documentation](https://github.com/bnfour/dotnet-telegram-forwarder#request) for more settings\.
        
        Example of a text message payload:
        ```
        {{
            "token": "{0}",
            "message": "{2}"
        }}
        ```
        Example of a sticker payload:
        ```
        {{
            "token": "{0}",
            "sticker": "{3}"
        }}
        ```
        If everything is okay, the API will return a blank 200 OK response\. If something is not okay, a different status code will be returned\. Consult [the documentation](https://github.com/bnfour/dotnet-telegram-forwarder#response) to see error code list\.
        """;
    }
}
