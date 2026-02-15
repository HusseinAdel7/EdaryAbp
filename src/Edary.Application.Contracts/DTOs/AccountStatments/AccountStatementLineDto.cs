using System;
using System.Collections.Generic;
using System.Text;

namespace Edary.DTOs.AccountStatments
{
    public class AccountStatementLineDto
    {
        public string MainAccountName { get; set; }
        public string SubAccountName { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Description { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? RunningBalance { get; set; }
        public int SortOrder { get; set; }
    }

}
