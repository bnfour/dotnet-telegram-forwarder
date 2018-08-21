using Microsoft.EntityFrameworkCore;

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
        /// Sets model parameters: makes UsageCounter and LastSuccessTimestamp .NET-only
        /// properties, marks token as a primary key. Also explicitly set table name
        /// to match actual DB file.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder to use.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Record>()
                .ToTable("Records")
                .Ignore(r => r.UsageCounter)
                .Ignore(r => r.LastSuccessTimestamp)
                .HasKey(r => r.Token);
        }
    }
}
