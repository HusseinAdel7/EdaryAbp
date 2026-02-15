using Edary.DTOs.AccountStatments;
using Edary.Entities.JournalEntries;
using Edary.Entities.MainAccounts;
using Edary.Entities.SubAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Edary.Services.AccountStatments
{
    public class AccountStatementManager : DomainService
    {
        private readonly IRepository<JournalEntryDetail, string> _detailRepo;
        private readonly IRepository<JournalEntry, string> _journalRepo;
        private readonly IRepository<SubAccount, string> _subRepo;
        private readonly IRepository<MainAccount, string> _mainRepo;

        public AccountStatementManager(
            IRepository<JournalEntryDetail, string> detailRepo,
            IRepository<JournalEntry, string> journalRepo,
            IRepository<SubAccount, string> subRepo,
            IRepository<MainAccount, string> mainRepo)
        {
            _detailRepo = detailRepo;
            _journalRepo = journalRepo;
            _subRepo = subRepo;
            _mainRepo = mainRepo;
        }

        public async Task<List<AccountStatementLineDto>> GenerateAsync(
            string accountId,
            DateTime fromDate,
            DateTime toDate)
        {
            if (string.IsNullOrWhiteSpace(accountId))
                throw new Exception("AccountId مطلوب");

            var subQuery = await _subRepo.GetQueryableAsync();
            var mainQuery = await _mainRepo.GetQueryableAsync();
            var detailQuery = await _detailRepo.GetQueryableAsync();
            var journalQuery = await _journalRepo.GetQueryableAsync();

            var subAccounts = subQuery
                .Where(x => x.Id == accountId || x.MainAccountId == accountId)
                .Select(x => x.Id);

            var movements = (
                from d in detailQuery
                join j in journalQuery on d.JournalEntryId equals j.Id
                join s in subQuery on d.SubAccountId equals s.Id
                join m in mainQuery on s.MainAccountId equals m.Id
                where subAccounts.Contains(s.Id)
                      && j.CreationTime >= fromDate
                      && j.CreationTime <= toDate
                orderby j.CreationTime, d.Id
                select new
                {
                    m.AccountName,
                    SubName = s.AccountName,
                    j.CreationTime,
                    d.Description,
                    d.Debit,
                    d.Credit
                }
            ).ToList();

            decimal running = 0;
            var result = new List<AccountStatementLineDto>();

            foreach (var item in movements)
            {
                running += item.Credit - item.Debit;

                result.Add(new AccountStatementLineDto
                {
                    MainAccountName = item.AccountName,
                    SubAccountName = item.SubName,
                    EntryDate = item.CreationTime,
                    Description = item.Description,
                    Debit = item.Debit,
                    Credit = item.Credit,
                    RunningBalance = running,
                    SortOrder = 1
                });
            }

            if (movements.Any())
            {
                result.Add(new AccountStatementLineDto
                {
                    Description = "إجمالي الفترة",
                    Debit = movements.Sum(x => x.Debit),
                    Credit = movements.Sum(x => x.Credit),
                    SortOrder = 2
                });

                result.Add(new AccountStatementLineDto
                {
                    Description = running > 0
                        ? "رصيد ختامي دائن"
                        : running < 0
                            ? "رصيد ختامي مدين"
                            : "رصيد ختامي",
                    RunningBalance = Math.Abs(running),
                    SortOrder = 3
                });
            }

            return result;
        }
    }
}
