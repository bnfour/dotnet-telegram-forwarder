# Dotnet Telegram forwarder  
_(also known as WebToTelegramCore earlier in development)_  

An app providing HTTP API to deliver arbitrary text or sticker notifications via associated Telegram bot from anywhere where making HTTP POST requests is available.  

## Status
Operational. First version has been serving me flawlessly (as I can tell) since mid 2018.

## Description
Let's try [readme driven development](http://tom.preston-werner.com/2010/08/23/readme-driven-development.html) this time. This app consists of two equally important parts: Telegram bot and a web API.

### Web API
Has only one method to send the notification. Its endpoint listens for POST requests with JSON body. Actual endpoint URL will be provided via the bot itself when a new token is created, and as part of `/token` command output.

#### Request
Request's body has this structure:
```
{
    "token": string,
    "silent": (optional) boolean,
    // either these for text notification
    "message": string,
    "type": (optional) string,
    // or this for sticker notification
    "sticker": string
}
```
* `token` is this service's user identifier, randomly generated per Telegram user, with abilities to withdraw it or replace with a new one anytime. It's a 16 characters long string that may contain alphanumerics, and plus and equals signs (So `[0-9a-zA-Z+=]{16}`).  
* `silent` is boolean to indicate whether them message from the bot in Telegram will come with a notification with sound. Behaves what you'd expect. If not supplied, defaults to `false`. Please note that the end user is able to mute the bot, effectively rendering this option useless.  

The next two parameters should be provided for a text notification:  
* `message` is the text of the message to be sent via the bot. Maximum length is 4096 (also happens to be a maximum length of one Telegram message).  
* `type` is used to select between two supported text parse modes: `"plaintext"` for plain text, and `"markdown"` for MarkdownV2 as described in [Telegram docs](https://core.telegram.org/bots/api#markdownv2-style). If value is not supplied, defaults to `"plaintext"`. These two are separated, because Telegram flavoured Markdown requires escaping for a fairly common plaintext punctuation marks, and will fail if not formed correctly.  

This parameter should be provided for a sticker notification:  
* `sticker` is an internal Telegram ID of a sticker. For obtaining values to use, see [Sticker identification](#sticker-identification).  

Providing both `message` and `sticker` and/or `type` and `sticker` at the same time is considered an invalid request.

#### Response
API returns an empty HTTP response with any of the following status codes:
* `200 OK` if everything is indeed OK and message should be expeted to be delivered via the bot  
No further actions from the client required.
* `400 Bad Request` if the user request is malformed and cannot be processed  
Client should check that the request is well-formed and meet the specifications, and retry with the fixed request.
* `404 Not Found` if supplied token is not present in the database  
Client should check that the token they provided is valid and has not been removed or changed, and retry with the correct one.
* `429 Too Many Requests` if current limit of sent messages is exhausted  
Client should retry later, after waiting at least one minute (on default throughput config).
* `500 Internal Server Error` in case anything goes wrong  
Client can try to retry later, but ¯\\\_(ツ)\_/¯

#### Rate limitation
The API has a rate limitation, preventing large amount of notifications in a short amount of time. By default _(can be adjusted via config files),_ every user has _20_ so-called message points. Every notification sent removes 1 message point, and requests will be refused with `429 Too Many Requests` status code when all points are depleted. A single point is regenerated every _minute_ after last message was sent.  
For instance, if API is used to send 40 notifications in quick succession, only the 20 first messages will be sent to the user. If the client waits 5 minutes after API starts responding with 429's, they will be able to send 5 more messages before hitting the limit again. After 20 minutes of idle time since the last successfully sent message, the API will behave as usual.

### Telegram bot
The bot is used both to deliver messages and to obtain token for requests for a given account.
Available commands:
* `/start` -- obligatory one, contains short description;
* `/help` -- also obligatory, displays output similar to this section;
* `/about` -- displays basic info about the bot, version and link to this repo;
* `/token` -- reminds user's token if there is one, also API usage hints and endpoint location;
* `/create` -- generates new token for user if none present;
* `/regenerate` -- once confirmed (see `/confirm` and `/cancel`) replaces user's token with a new one;
* `/delete` -- once confirmed removes user's token;
* `/confirm` -- confirms regeneration or deletion;
* `/cancel` -- cancels regeneration or deletion.  

When there is a destructive (either regeneration or deletion) operation pending, only `/cancel` or `/confirm` commands are accepted.

#### Registration limitation
The ability for anyone to create a token for themselves is toggleable via config entry. You can always run direct queries against bot's DB for quick editing. Note that messages will not be delivered unless user actually engaged in a conversation with the bot if an entry was created via DB edits.

#### Sticker identification
Bot will reply with a message containing the ID of any sticker sent to it. This value could be used to send notifications containing a sticker instead of text.  
This works even when there is a pending confirmation which usually blocks most of the commands.

## Configuration and deployment
You'll need .NET 7 runtime. Hosting on GNU/Linux in a proper data center is highly encouraged.  
By default, the app listens on port 8082. This can be changed with `--port <desired port>` command-line argument.  
Rest of the settings are stored inside `appsettings.json`:  
* "General" section contains Telegram's bot token, API endpoint URL as seen from outside world (certainly not localhost:8082 as Kestrel would told you it listens to) and a boolean that controls whether new users can create tokens. Please note that `/api` will be appended to this address. So, for example, if you set `https://foo.example.com/bar` here, actual endpoint to be used with the API will be `https://foo.example.com/bar/api`, Telegram webhook endpoint will be `https://foo.example.com/bar/api/{bot-token}`  .
* "Bandwidth" section controls bot's [throughput](#rate-limitation): maximum amount of messages to be delivered at once and amount of seconds to regenerate one message delivery is set here.  

To deploy this bot, you'll need something that will append SSL as required by Telegram. As always with Telegram bots, I recommend `nginx` as a reverse proxy. You'll need to set up HTTPS as well.

## Version history
* **v 1.0**, 2018-08-29  
Initial release. More an excercise in ASP.NET Core than an app I needed at the moment. Actually helped me to score a software engineering job and turned out to be moderately useful tool.
* **v 1.2**, 2018-10-05  
Greatly increased the reliability of Markdown parsing in one of the most **not** straightforward ways you can imagine -- by converting the Markdown to HTML with a few custom convertion quirks.
* **no version number**, 2020-05-14  
Shelved attempt to improve the codebase. Consists of one architecture change and is fully ~~included~~ rewritten in the next release.
* **v 2.0**, 2023-05-06  
Really proper markdown support this time (Telegram's version with questionable selection of characters to be escaped), option to send a silent notification, async everthing, .NET 7, HTTP status codes instead of custom errors, and probably something else I forgot about.
* **v 2.1**, not yet released, 2023-05 probably    
Support for sending stickers instead of text messages.
