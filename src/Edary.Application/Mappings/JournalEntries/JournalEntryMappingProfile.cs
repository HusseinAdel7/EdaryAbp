using AutoMapper;
using Edary.DTOs.JournalEntries;
using Edary.Entities.JournalEntries;
using System;

namespace Edary.Mappings.JournalEntries
{
    public class JournalEntryMappingProfile : Profile
    {
        public JournalEntryMappingProfile()
        {
            CreateMap<CreateJournalEntryDto, JournalEntry>();
            CreateMap<UpdateJournalEntryDto, JournalEntry>();
            CreateMap<CreateJournalEntryDetailDto, JournalEntryDetail>();
            CreateMap<UpdateJournalEntryDetailDto, JournalEntryDetail>();
            CreateMap<JournalEntry, JournalEntryDto>();
            CreateMap<JournalEntryDetail, JournalEntryDetailDto>();

        }
    }
}
