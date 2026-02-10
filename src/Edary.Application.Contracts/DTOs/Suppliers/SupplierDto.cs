using System;
using Volo.Abp.Application.Dtos;

namespace Edary.DTOs.Suppliers
{
    public class SupplierDto : FullAuditedEntityDto<string>
    {
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string? SubAccountId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxNumber { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }
        public string SupplierNameEn { get; set; }
        public Guid? TenantId { get; set; }
    }
}

