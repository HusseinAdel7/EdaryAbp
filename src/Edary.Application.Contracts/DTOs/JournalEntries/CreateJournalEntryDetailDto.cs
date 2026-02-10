using System;
using System.ComponentModel.DataAnnotations;

namespace Edary.DTOs.JournalEntries
{
    public class CreateJournalEntryDetailDto
    {
        public string? SubAccountId { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
