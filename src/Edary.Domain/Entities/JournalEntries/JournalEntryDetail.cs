using Edary.Entities.SubAccounts;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Edary.Entities.JournalEntries
{
    public class JournalEntryDetail : FullAuditedEntity<string>, IMultiTenant
    {
        protected JournalEntryDetail() { }

        public JournalEntryDetail(string id)
        {
            Id = id;
        }

        public string? JournalEntryId { get; set; }
        public string? SubAccountId { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public JournalEntry JournalEntry { get; set; }

        // Navigation properties
        public SubAccount SubAccount { get; set; }

        public Guid? TenantId { get; set; }
    }
}

