namespace WebToTelegramCore.Options
{
    /// <summary>
    /// Class that holds all customizable string the bot may reply with.
    /// Also, descriptions of error codes.
    /// </summary>
    public class LocalizationOptions
    {
        /// <summary>
        /// Message to show when token deletion was cancelled.
        /// </summary>
        public string CancelDeletion { get; set; }
        
        /// <summary>
        /// Message to show when token regeneration was cancelled.
        /// </summary>
        public string CancelRegeneration { get; set; }

        /// <summary>
        /// Message to show when token deletion was completed.
        /// </summary>
        public string ConfirmDeletion { get; set; }

        /// <summary>
        /// Template for message to show when token regeneration was completed.
        /// {0} is new token.
        /// </summary>
        public string ConfirmRegeneration { get; set; }
        
        /// <summary>
        /// Message to reply to /create with when registration is disabled.
        /// </summary>
        public string CreateGoAway { get; set; }
        
        /// <summary>
        /// Template for message to show when token was created successfully.
        /// {0} is new token.
        /// </summary>
        public string CreateSuccess { get; set; }
        
        /// <summary>
        /// Message to show when user initiated token deletion and registration is off.
        /// </summary>
        public string DeletionNoTurningBack { get; set; }

        /// <summary>
        /// Message to show when user initiated token deletion.
        /// </summary>
        public string DeletionPending { get; set; }
        
        /// <summary>
        /// Message to show when confirmation is pending and user tries to use
        /// non-confirming command.
        /// </summary>
        public string ErrorConfirmationPending { get; set; }
        
        /// <summary>
        /// Message to show to unknown commands starting with slash.
        /// </summary>
        public string ErrorDave { get; set; }
        
        /// <summary>
        /// Message to show when usage of command requires no token set,
        /// but user has one.
        /// </summary>
        public string ErrorMustBeGuest { get; set; }
        
        /// <summary>
        /// Message to show when usage of command requires a token, but user has none.
        /// </summary>
        public string ErrorMustBeUser { get; set; }
        
        /// <summary>
        /// Message to show when user is trying to use confirming command, but no
        /// confirmation is pending.
        /// </summary>
        public string ErrorNoConfirmationPending { get; set; }
        
        /// <summary>
        /// Message to show on unknown input.
        /// </summary>
        public string ErrorWhat { get; set; }
        
        /// <summary>
        /// Output of /help command.
        /// </summary>
        public string Help { get; set; }
        
        /// <summary>
        /// Message to show when user initiated token regeneration.
        /// </summary>
        public string RegenerationPending { get; set; }
        
        /// <summary>
        /// Human-readable representation of ResponseState.BandwidthExceeded enum member.
        /// </summary>
        public string RequestBandwidthExceeded { get; set; }

        /// <summary>
        /// Human-readable representation of ResponseState.NoSuchToken enum member.
        /// </summary>
        public string RequestNoToken { get; set; }

        /// <summary>
        /// Human-readable representation of ResponseState.OkSent enum member.
        /// </summary>
        public string RequestOk { get; set; }

        /// <summary>
        /// Human-readable representation of ResponseState.SomethingBadHappened
        /// enum member.
        /// </summary>
        public string RequestWhat { get; set; }
        
        /// <summary>
        /// Message to warn new users that registration is closed.
        /// </summary>
        public string StartGoAway { get; set; }
        
        /// <summary>
        /// Message to show when user first engages the bot.
        /// </summary>
        public string StartMessage { get; set; }
        
        /// <summary>
        /// Message to encourage new users to register.
        /// </summary>
        public string StartRegistrationHint { get; set; }
        
        /// <summary>
        /// Helper text that explains web API output.
        /// </summary>
        public string TokenErrorsDescription { get; set; }
        
        /// <summary>
        /// Template for message to reply to /token command with.
        /// {0} is token, {1} is API endpoint URL, {2} is vanity quote.
        /// </summary>
        public string TokenTemplate { get; set; }
    }
}
