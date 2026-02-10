using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.Suppliers
{
    public class SupplierPagedRequestDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public bool? IsActive { get; set; }
        public string? SubAccountId { get; set; }
    }
}

