using Edary.Entities.JournalEntries;
using Edary.Consts.JournalEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edary.EntityFrameworkCore.EntitiesConfigurations.JournalEntries
{
    public class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
    {
        public void Configure(EntityTypeBuilder<JournalEntry> builder)
        {
            builder.ToTable("JournalEntries", schema: "JournalEntries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(JournalEntryConsts.MaxCurrencyLength);

            builder.Property(x => x.ExchangeRate).IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(JournalEntryConsts.MaxNotesLength);

      

            builder.Property(x => x.CurrencyEn)
                .HasMaxLength(JournalEntryConsts.MaxCurrencyEnLength);

            builder.HasMany(x => x.JournalEntryDetails)
                .WithOne(x => x.JournalEntry)
                .HasForeignKey(x => x.JournalEntryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

