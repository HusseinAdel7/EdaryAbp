using System.Linq;
using System.Threading.Tasks;
using Edary.Entities.Suppliers;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace Edary.Domain.Services.Suppliers
{
    public class SupplierManager : DomainService
    {
        private readonly IRepository<Supplier, string> _supplierRepository;

        public SupplierManager(IRepository<Supplier, string> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [UnitOfWork]
        public virtual async Task<string> GenerateNewSupplierCodeAsync()
        {
            var queryable = await _supplierRepository.GetQueryableAsync().ConfigureAwait(false);

            // SupplierCode is unique (see configuration). We generate next numeric code as string.
            var maxCode = queryable
                .Select(s => s.SupplierCode)
                .OrderByDescending(c => c)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(maxCode))
            {
                return "1";
            }

            if (!long.TryParse(maxCode, out var maxValue))
            {
                throw new BusinessException("Edary:InvalidSupplierCodeFormat",
                    $"Cannot generate new SupplierCode because existing max code '{maxCode}' is not numeric.");
            }

            return (maxValue + 1).ToString();
        }
    }
}

