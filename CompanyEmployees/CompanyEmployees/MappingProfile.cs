using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace CompanyEmployees;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Company, CompanyDto>()
        //    .ForMember(dto => dto.FullAddress,
        //        opt => opt.MapFrom(c => string.Join(' ', c.Address, c.Country)));

        CreateMap<Company, CompanyDto>()
            .ForCtorParam("FullAddress",
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

        CreateMap<Employee, EmployeeDto>();
    }
}