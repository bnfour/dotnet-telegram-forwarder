# Dotnet Telegram forwarder  
(temporary name that stuck since I can't think of a better one)    
An ASP.NET Core app providing HTTP API for notifications via Telegram bot.  

## What's this?
My attempt to grasp the basics of both ASP.NET and EF by writing moderately useful app.  
Telegram bot is used to provide auth tokens to users and to actually notify them when something makes a request to the provided web API. 
This can be used to deliver all kinds of notifications via Telegram.  

## Status
Pretty much works on my server (sorry, registration's closed). Your mileage may vary.  
I still have to write some actual clients though.

## Description
Let's try [readme driven development](http://tom.preston-werner.com/2010/08/23/readme-driven-development.html) this time. So this app consists of two parts: Telegram bot and a web API.

### Web API
Has only one method to send the notification. Its endpoint listens for requests with JSON body containing auth token and message to deliver. (Messages should support markdown.)  
Request should look like that:
```
{
	"token": "basically `[0-9a-zA-Z+=]{16}`>",
	"message": "text to be delivered via bot"
}
```
Tokens are tied to Telegram IDs internally, also there is limitations on how often messages can be sent: every token has up to 20 instant deliveries, 
with one regenerating every minute after last successful message delivery.  
If request was correctly formed, API will respond with another JSON object:  
```
{
	"ok": boolean, (if true message is accepted. If false, some kind of error happened),
	"code": int, (represents error codes in machine-friendly format),
	"details": string (human-friendly error description)
}
```
Possible error codes:
* 0 -- No error, message sent. Used when "ok" is true;
* 1 -- No such token. Token is in valid format but is not found in the database;
* 2 -- Bandwidth exceeded. Too many messages in a given amount of time. Client should wait at least a minute (by default) before retrying.  

If request isn't in correct format, blank 400 Bad Request is thrown instead.


### Telegram bot
The bot is used both to deliver messages and to obtain token for requests for a given account.
It has some commands:
* `/start` -- obligatory one, contains short description;
* `/help` -- also obligatory, displays output similar to this text;
* `/about` -- displays basic info about the bot, like link back to this repo;
* `/token` -- reminds user's token if there is one, also API usage hints;
* `/create` -- generates new token for user if none present;
* `/regenerate` -- once confirmed (see `/confirm` and `/cancel`) replaces user's token with a new one;
* `/delete` -- once confirmed removes user's token;
* `/confirm` -- confirms regeneration or deletion;
* `/cancel` -- cancels regeneration or deletion.  

When there is destructive (either regeneration or deletion) operation pending after initial command, only `/cancel` or `/confirm` commands are
accepted.

## Configuration and deployment
By default, listens on port 8082. This can be changed with `--port <desired port>` command-line argument.  
Rest of the settings are inside `appsettings.json`:  
* "Strings" section contains all the customizable strings just in case localization is ever needed.  
* "General" section contains Telegram's bot token, API endpoint URL as seen from outside world (certainly not localhost:8082 as Kestrel would told you it listens to) and a boolean that controls whether new users can create tokens.  
* "Bandwidth" section controls bot's throughput: maximum amount of messages to be delivered at once and amount of seconds to regenerate one message delivery is set here.  
To deploy this bot, you'll need something that will append SSL as required by Telegram. As always, I recommend `nginx` as a reverse proxy. Another quirk is that launching via `./WebToTelegramCore`
didn't worked with my setup: server had ASP.NET Core 2.1.3 runtime, but app was expecting 2.1.2. `dotnet WebToTelegramCore.dll` worked though.

## Possible TODO
Make a companion website that also allows token manipulation just like the bot?