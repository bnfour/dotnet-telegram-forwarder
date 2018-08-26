# Dotnet Telegram forwarder (name pending)  
An ASP.NET Core app providing HTTP API for notifications via Telegram bot.  

## What's this?
My attempt to grasp the basics of both ASP.NET and EF by writing moderately useful app.  
Telegram bot is used to provide auth tokens to users and to actually notify them when something makes a request to the provided web API. 
This can be used to deliver all kinds of notifications via Telegram.  
Pretty much work in progress, no idea when (or even if ever) this will be finished.

## Status
Pretty much works in closed beta, main task now is to move hardcoded strings to config file, then write deployment instructions.  
Also test what happens with malformed input.

## Description
Let's try [readme driven development](http://tom.preston-werner.com/2010/08/23/readme-driven-development.html) this time. So this app consists of two parts: Telegram bot and a web API.

### Web API
Has only one method to send the notification. Its endpoint listens for requests with JSON body containing auth token and message to deliver. (Messages should support markdown.)  
JSON like that:
```
{
	"token": "long enough random string",
	"message": "text to be delivered via bot"
}
```
Tokens are tied to Telegram IDs internally, also there is limitations on how often messages can be sent: every token has up to 20 instant deliveries, 
with one regenerating every minute.  

### Telegram bot
The bot is used both to deliver messages and to obtain token for requests for a given account.
It has some commands:
* `/start` -- obligatory one, contains short description;
* `/token` -- reminds user's token if there is one;
* `/create` -- generates new token for user if none present;
* `/regenerate` -- once confirmed (see `/confirm` and `/cancel`) replaces user's token with a new one;
* `/delete` -- once confirmed removes user's token;
* `/confirm` -- confirms regeneration or deletion;
* `/cancel` -- cancels regeneration or deletion.  

When there is destructive (either regeneration or deletion) operation pending after initial command, only `/cancel` or `/confirm` commands are
accepted.

### Realization
Via ASP.NET Core (maybe even MVC with these fancy dependency injections) and .NET wrapper for Telegram bot API. Backend for storing
auth token - Telegram account ID relationship is SQLite database accessed via EF Core.  
The "Core" parts are important as I can only host this thing on my Debian VPS.