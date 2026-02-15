using Edary.DTOs.AccountStatments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Edary.IAppServices
{

    namespace Edary.IAppServices
    {
        public interface IAccountStatementAppService : IApplicationService
        {
            Task<List<AccountStatementLineDto>> GetAsync(AccountStatementInputDto input);
        }
    }
}
