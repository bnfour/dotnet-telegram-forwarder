# Dotnet Telegram forwarder  
("temporary" generic name from the very start of development that stuck forever)  
An app providing HTTP API to deliver arbitrary text notifications via Telegram bot from anywhere where making HTTP POST requests is available.  

## Status
Operational. First version has been serving me flawlessly (as I can tell) since mid 2018, and a long overdue refactoring/framework update (spoiler: one of the major updates two planned) is currently underway.

## Description
Let's try [readme driven development](http://tom.preston-werner.com/2010/08/23/readme-driven-development.html) this time. So this app consists of two equally important parts: Telegram bot and a web API.

### Web API
Has only one method to send the notification. Its endpoint listens for POST requests with JSON body. Actual endpoint URL will be provided via the bot itself when a new token is created.
#### Request
Request's body has this structure:
```
{
    "token": string,
    "type": (optional) string,
    "message": string,
    "silent": (optional) boolean
}
```
* Token is this service's user identifier, randomly generated per Telegram user, with abilities to withdraw it or replace with a new one anytime. (Implement a cooldown on consequent resets?) It's a 16 characters long string that may contain alphanumerics, and plus and equals signs (So `[0-9a-zA-Z+=]{16}`).  
* Type is used to select between two supported parse modes: `"plaintext"` for plain text, and `"markdown"` for MarkdownV2 as described in Telegram docs [here](https://core.telegram.org/bots/api#markdownv2-style). If value is not supplied, defaults to `"plaintext"`. These two are separated, because Telegram flavoured Markdown requires escaping for a fairly common plaintext punctuation marks, and Telegram backend (from my experience three years ago, actualize) tends to silently drop malformed Markdown.  
* Message is the text of the message to be sent via the bot. Maximum length is 4096, and preferred encoding is UTF-8.  
* Silent is boolean to indicate whether them message from the bot in Telegram will come with a notification. Behaves what you'd expect. If not supplied, defaults to `false`. Please note that the end user is able to mute the bot, effectively rendering this option useless.

#### Response
API returns an empty HTTP response with any of the following status codes:
* `200 OK` if everything is indeed OK and message should be expeted to be delivered via the bot  
No further actions from the client required
* `400 Bad Request` if the user request is malformed and cannot be processed  
Client should check that the request is well-formed and meet the specifications, and retry with the fixed request
* `404 Not Found` if supplied token is not present in the database  
Client should check that the token they provided is valid and has not been removed via commands, and retry with the correct one
* `429 Too Many Requests` if current limit of messages sent is exhausted  
Client should retry later, after waiting at least one minute (on default throughput config)
* `500 Internal Server Error` in case anything goes wrong  
Client can try to retry later, but ¯\\\_(ツ)\_/¯

#### Rate limitation
The API has a rate limitation, preventing large(ish) amount of notifications in a short amount of time. By default (can be adjusted via config files), every user has 20 ...message points, I guess? Every notification sent removes 1 message point, and requests will be refused with `429 Too Many Requests` status code when points are depleted. A single point is regenerated every minute after last message was sent.  
For instance, if API is used to send 40 notifications in quick succession, only 20 first messages will be sent to the user. If client waits 5 minutes after API starts responding with 429's, they will be able to send 5 more messages instantenously before hitting the limit again. After 20 minutes of idle time since the last successfully sent message, the API will behave as usual.

### Telegram bot
The bot is used both to deliver messages and to obtain token for requests for a given account.
It has some commands:
* `/start` -- obligatory one, contains short description;
* `/help` -- also obligatory, displays output similar to this section;
* `/about` -- displays basic info about the bot, like link back to this repo;
* `/token` -- reminds user's token if there is one, also API usage hints and endpoint location;
* `/create` -- generates new token for user if none present;
* `/regenerate` -- once confirmed (see `/confirm` and `/cancel`) replaces user's token with a new one;
* `/delete` -- once confirmed removes user's token;
* `/confirm` -- confirms regeneration or deletion;
* `/cancel` -- cancels regeneration or deletion.  

The ability for anyone to create a token for themselves is toggleable via config entry. You can always run direct queries against bot's DB for quick editing. Note that messages will not be delivered unless user actually engaged in a conversation with the bot.

When there is a destructive (either regeneration or deletion) operation pending, only `/cancel` or `/confirm` commands are accepted.

## Configuration and deployment
You'll need .NET 6 runtime. Hosting on GNU/Linux in a proper data center is highly encouraged.  
By default, listens on port 8082. This can be changed with `--port <desired port>` command-line argument.  
Rest of the settings are inside `appsettings.json`:  
* "General" section contains Telegram's bot token, API endpoint URL as seen from outside world (certainly not localhost:8082 as Kestrel would told you it listens to) and a boolean that controls whether new users can create tokens.  
* "Bandwidth" section controls bot's throughput: maximum amount of messages to be delivered at once and amount of seconds to regenerate one message delivery is set here.  

// TODO write about localization string moved somewhere else

To deploy this bot, you'll need something that will append SSL as required by Telegram. As always with Telegram bots, I recommend `nginx` as a reverse proxy. You'll need to set up HTTPS as well.

### Debug
Short section outlining ngrok usage?

## Version history
* **v 1.0**, 2018-08-29  
Initial release. More an excercise in ASP.NET Core than an app I needed at the moment. Actually helped me to score a software engineering job and turned out to be moderately useful tool.
* **v 1.2**, 2018-10-05  
Greatly increased the reliability of Markdown parsing in one of the most **not** straightforward ways you can imagine -- by converting the Markdown to HTML with a few custom convertion quirks.
* **no version number**, 2020-05-14  
Shelved attempt to improve the codebase. Consists of one architecture change and is fully included in the next release.
* TODO the current iteration, version number 1.5? 1.9? Certainly not 2.x _yet_.