using Edary.DTOs.JournalEntries;
using Edary.Entities.JournalEntries;
using Edary.IAppServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace Edary.AppServices.JournalEntries
{
    public class JournalEntryAppService : 
        CrudAppService<JournalEntry, JournalEntryDto, string, GetJournalEntryListInput, CreateJournalEntryDto, UpdateJournalEntryDto>, 
        IJournalEntryAppService
    {
        private readonly IRepository<JournalEntryDetail, string> _journalEntryDetailRepository;
        private readonly IRepository<JournalEntry, string> _journalEntryRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;



        public JournalEntryAppService(
            IRepository<JournalEntry, string> journalEntryRepository,
            IRepository<JournalEntryDetail, string> journalEntryDetailRepository,
            IGuidGenerator guidGenerator,
            ICurrentUser currentUser)
            : base(journalEntryRepository)
        {
            _journalEntryRepository = journalEntryRepository;
            _journalEntryDetailRepository = journalEntryDetailRepository;
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;

        }

        public override async Task<JournalEntryDto> GetAsync(string id)
        {
            var journalEntry = await _journalEntryRepository.WithDetailsAsync(x => x.JournalEntryDetails);
            return ObjectMapper.Map<JournalEntry, JournalEntryDto>(journalEntry.FirstOrDefault(x => x.Id == id));
        }

        public override async Task<PagedResultDto<JournalEntryDto>> GetListAsync(GetJournalEntryListInput input)
        {
            var query = await _journalEntryRepository.WithDetailsAsync(x => x.JournalEntryDetails);

            query = query
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
                    journalEntry => journalEntry.Currency.Contains(input.Filter) ||
                                    journalEntry.Notes.Contains(input.Filter) 
                                    )
                .WhereIf(!input.Currency.IsNullOrWhiteSpace(), 
                    journalEntry => journalEntry.Currency.Contains(input.Currency))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), 
                    journalEntry => journalEntry.Notes.Contains(input.Notes));

            var totalCount = await AsyncExecuter.CountAsync(query);

            query = query.OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "Currency asc" : input.Sorting)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount);

            var journalEntries = await AsyncExecuter.ToListAsync(query);

            return new PagedResultDto<JournalEntryDto>(totalCount, ObjectMapper.Map<List<JournalEntry>, List<JournalEntryDto>>(journalEntries));
        }

        public override async Task<JournalEntryDto> CreateAsync(CreateJournalEntryDto input)
        {

            var journalEntry = new JournalEntry(_guidGenerator.Create().ToString());

            ObjectMapper.Map(input, journalEntry);

        

            foreach (var detail in journalEntry.JournalEntryDetails)
            {
                detail.GetType()
                      .GetProperty("Id")
                      ?.SetValue(detail, _guidGenerator.Create().ToString());

                detail.JournalEntryId = journalEntry.Id;
            }

            await _journalEntryRepository.InsertAsync(journalEntry, autoSave: true);

            return ObjectMapper.Map<JournalEntry, JournalEntryDto>(journalEntry);
        }

        public override async Task<JournalEntryDto> UpdateAsync(string id, UpdateJournalEntryDto input)
        {
            var journalEntry = await _journalEntryRepository.GetAsync(id);
            ObjectMapper.Map(input, journalEntry);

            // Handle JournalEntryDetails updates
            foreach (var detailDto in input.JournalEntryDetails)
            {
                if (detailDto.Id == null)
                {
                    // New detail
                    var newDetail = ObjectMapper.Map<UpdateJournalEntryDetailDto, JournalEntryDetail>(detailDto);
                    newDetail.JournalEntryId = id;
                    await _journalEntryDetailRepository.InsertAsync(newDetail);
                }
                else
                {
                    // Existing detail
                    var existingDetail = await _journalEntryDetailRepository.GetAsync(detailDto.Id);
                    ObjectMapper.Map(detailDto, existingDetail);
                    await _journalEntryDetailRepository.UpdateAsync(existingDetail);
                }
            }

            // Remove details not present in the input
            var detailIdsToRemove = journalEntry.JournalEntryDetails.Select(x => x.Id)
                                                .Except(input.JournalEntryDetails.Where(x => x.Id != null).Select(x => x.Id))
                                                .ToList();

            foreach (var detailId in detailIdsToRemove)
            {
                await _journalEntryDetailRepository.DeleteAsync(detailId);
            }
            
            await _journalEntryRepository.UpdateAsync(journalEntry, autoSave: true);
            return ObjectMapper.Map<JournalEntry, JournalEntryDto>(journalEntry);
        }

        public override async Task DeleteAsync(string id)
        {
            // Delete associated JournalEntryDetails first
            var detailIdsToDelete = await (await _journalEntryDetailRepository.GetQueryableAsync())
                .Where(x => x.JournalEntryId == id)
                .Select(x => x.Id)
                .ToListAsync();
            await _journalEntryDetailRepository.DeleteManyAsync(detailIdsToDelete);
            await _journalEntryRepository.DeleteAsync(id);
        }
    }
}
