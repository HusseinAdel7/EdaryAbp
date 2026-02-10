using Edary.DTOs.Suppliers;
using Volo.Abp.Application.Services;

namespace Edary.IAppServices
{
    public interface ISupplierAppService :
        ICrudAppService<
            SupplierDto,
            string,
            SupplierPagedRequestDto,
            CreateSupplierDto,
            UpdateSupplierDto>
    {
    }
}

