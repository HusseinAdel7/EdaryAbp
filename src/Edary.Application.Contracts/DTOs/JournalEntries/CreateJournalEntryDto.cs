using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Edary.DTOs.JournalEntries
{
    public class CreateJournalEntryDto
    {
        [Required]
        public string Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string Notes { get; set; }

        public string CurrencyEn { get; set; }

        [MinLength(2, ErrorMessage = "A Journal Entry must have at least two details.")]
        public ICollection<CreateJournalEntryDetailDto> JournalEntryDetails { get; set; }

        public CreateJournalEntryDto()
        {
            JournalEntryDetails = new HashSet<CreateJournalEntryDetailDto>();
        }
    }
}
