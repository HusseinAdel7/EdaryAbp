using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.AccountStatments
{
    public class AccountStatementInputDto : PagedAndSortedResultRequestDto
    {
        public string AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

}
