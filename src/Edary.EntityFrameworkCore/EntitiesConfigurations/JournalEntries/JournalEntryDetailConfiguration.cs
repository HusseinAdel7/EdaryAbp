using Edary.Entities.JournalEntries;
using Edary.Entities.SubAccounts;
using Edary.Consts.JournalEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edary.EntityFrameworkCore.EntitiesConfigurations.JournalEntries
{
    public class JournalEntryDetailConfiguration : IEntityTypeConfiguration<JournalEntryDetail>
    {
        public void Configure(EntityTypeBuilder<JournalEntryDetail> builder)
        {
            builder.ToTable("JournalEntryDetails", schema: "JournalEntries");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasMaxLength(36)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(JournalEntryDetailConsts.MaxDescriptionLength)
                   .IsRequired();

            builder.Property(x => x.Debit).IsRequired();
            builder.Property(x => x.Credit).IsRequired();

            // ✅ JournalEntryDetail -> JournalEntry (Required)
            builder.HasOne(x => x.JournalEntry)
                   .WithMany(x => x.JournalEntryDetails)
                   .HasForeignKey(x => x.JournalEntryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // ✅ JournalEntryDetail -> SubAccount (Optional)
            builder.HasOne(x => x.SubAccount)
                   .WithMany()
                   .HasForeignKey(x => x.SubAccountId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
