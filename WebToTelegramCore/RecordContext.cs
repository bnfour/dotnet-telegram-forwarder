using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebToTelegramCore.Models;

namespace WebToTelegramCore
{
    /// <summary>
    /// Class that encapsulates Records for EF usage.
    /// </summary>
    public class RecordContext : DbContext
    {
        /// <summary>
        /// Record collection for manipulation within the app.
        /// </summary>
        public DbSet<Record> Records { get; private set; }

        /// <summary>
        /// Constructor that does literally nothing
        /// but is required anyway due to base call.
        /// </summary>
        /// <param name="options">Options for the context.</param>
        public RecordContext(DbContextOptions<RecordContext> options) : base(options) { }

        /// <summary>
        /// Sets model parameters: makes UsageCounter, LastSuccessTimestamp, and State
        /// as .NET-only properties, marks token as a primary key. Also explicitly
        /// sets table name to match actual DB file.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .ToTable("Records")
                .Ignore(r => r.UsageCounter)
                .Ignore(r => r.LastSuccessTimestamp)
                .Ignore(r => r.State)
                .HasKey(r => r.Token);
        }

        /// <summary>
        /// Tries to fetch record by token from the database.
        /// </summary>
        /// <param name="token">Token to search for in the DB.</param>
        /// <returns>Associated Record or null if none found.</returns>
        public async Task<Record> GetRecordByToken(string token)
        {
            return await Records.SingleOrDefaultAsync(r => r.Token.Equals(token));
        }

        /// <summary>
        /// Gets Record associated with a given Telegram ID from the database.
        /// </summary>
        /// <param name="accountId">ID to search for.</param>
        /// <returns>Associated Record or null if none present.</returns>
        public async Task<Record> GetRecordByAccountId(long accountId)
        {
            return await Records.SingleOrDefaultAsync(r => r.AccountNumber == accountId);
        }
    }
}
