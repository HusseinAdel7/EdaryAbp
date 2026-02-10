using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.JournalEntries
{
    public class JournalEntryDto : FullAuditedEntityDto<string>
    {
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Notes { get; set; }
        public string CurrencyEn { get; set; }
        public Guid? TenantId { get; set; }

        public ICollection<JournalEntryDetailDto> JournalEntryDetails { get; set; }
    }
}
