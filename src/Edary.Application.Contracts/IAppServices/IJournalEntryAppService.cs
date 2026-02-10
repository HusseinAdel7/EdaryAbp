using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Edary.DTOs.JournalEntries;

namespace Edary.IAppServices
{
    public interface IJournalEntryAppService : 
        ICrudAppService< //Defines CRUD methods
            JournalEntryDto, //Used to show books
            string, //Primary key of the book entity
            GetJournalEntryListInput, //Used for paging/sorting
            CreateJournalEntryDto, //Used to create a book
            UpdateJournalEntryDto> //Used to update a book
    {
        // Add any custom methods here if needed
    }
}
