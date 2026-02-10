using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.JournalEntries
{
    public class GetJournalEntryListInput : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public string? Currency { get; set; }
        public string? Notes { get; set; }
    }
}
