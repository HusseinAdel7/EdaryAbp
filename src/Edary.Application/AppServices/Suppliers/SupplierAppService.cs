using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Edary.Domain.Services.Suppliers;
using Edary.DTOs.Suppliers;
using Edary.Entities.Suppliers;
using Edary.IAppServices;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Edary.AppServices.Suppliers
{
    public class SupplierAppService :
        CrudAppService<
            Supplier,
            SupplierDto,
            string,
            SupplierPagedRequestDto,
            CreateSupplierDto,
            UpdateSupplierDto>,
        ISupplierAppService
    {
        private readonly SupplierManager _supplierManager;

        public SupplierAppService(
            IRepository<Supplier, string> repository,
            SupplierManager supplierManager)
            : base(repository)
        {
            _supplierManager = supplierManager;
        }

        public override async Task<SupplierDto> CreateAsync(CreateSupplierDto input)
        {
            var newSupplierId = GuidGenerator.Create().ToString();
            var newSupplierCode = await _supplierManager.GenerateNewSupplierCodeAsync();

            var supplier = ObjectMapper.Map<CreateSupplierDto, Supplier>(input);

            // Entity<string>.Id setter is protected; use ABP helper (same pattern used by framework)
            EntityHelper.TrySetId(supplier, () => newSupplierId);

            // SupplierCode is auto-generated (NOT from user)
            supplier.SupplierCode = newSupplierCode;

            var created = await Repository.InsertAsync(supplier, autoSave: true);
            return MapToGetOutputDto(created);
        }

        public override async Task<SupplierDto> UpdateAsync(string id, UpdateSupplierDto input)
        {
            var supplier = await Repository.GetAsync(id);

            // SupplierCode must not be changed by user
            supplier.SupplierName = input.SupplierName;
            supplier.SubAccountId = input.SubAccountId;
            supplier.Phone = input.Phone;
            supplier.Email = input.Email;
            supplier.Address = input.Address;
            supplier.TaxNumber = input.TaxNumber;
            supplier.Notes = input.Notes;
            supplier.IsActive = input.IsActive;
            supplier.SupplierNameEn = input.SupplierNameEn;

            var updated = await Repository.UpdateAsync(supplier, autoSave: true);
            return MapToGetOutputDto(updated);
        }

        public override async Task<PagedResultDto<SupplierDto>> GetListAsync(SupplierPagedRequestDto input)
        {
            var query = await Repository.GetQueryableAsync();

            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(s =>
                    s.SupplierCode.Contains(input.Filter) ||
                    s.SupplierName.Contains(input.Filter) ||
                    (s.SupplierNameEn != null && s.SupplierNameEn.Contains(input.Filter)) ||
                    (s.Phone != null && s.Phone.Contains(input.Filter)) ||
                    (s.Email != null && s.Email.Contains(input.Filter)) ||
                    (s.TaxNumber != null && s.TaxNumber.Contains(input.Filter))
                );
            }

            if (input.IsActive.HasValue)
            {
                query = query.Where(s => s.IsActive == input.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(input.SubAccountId))
            {
                query = query.Where(s => s.SubAccountId == input.SubAccountId);
            }

            query = !string.IsNullOrWhiteSpace(input.Sorting)
                ? query.OrderBy(input.Sorting)
                : query.OrderByDescending(s => s.CreationTime);

            var totalCount = await AsyncExecuter.CountAsync(query);
            query = query.PageBy(input.SkipCount, input.MaxResultCount);

            var entities = await AsyncExecuter.ToListAsync(query);
            var dtos = entities.Select(MapToGetOutputDto).ToList();

            return new PagedResultDto<SupplierDto>(totalCount, dtos);
        }
    }
}

