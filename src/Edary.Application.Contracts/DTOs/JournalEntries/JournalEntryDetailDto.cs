using System;
using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.JournalEntries
{
    public class JournalEntryDetailDto : FullAuditedEntityDto<string>
    {
        public string? JournalEntryId { get; set; }
        public string? SubAccountId { get; set; }
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public Guid? TenantId { get; set; }
    }
}
