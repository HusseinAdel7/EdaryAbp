using AutoMapper;
using Edary.DTOs.Suppliers;
using Edary.Entities.Suppliers;

namespace Edary.Application.Mappings.Suppliers
{
    public class SupplierMappingProfile : Profile
    {
        public SupplierMappingProfile()
        {
            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<UpdateSupplierDto, Supplier>();
        }
    }
}

