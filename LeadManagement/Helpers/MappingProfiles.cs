using AutoMapper;
using LeadManagement.Data.Dtos;
using LeadManagement.Models;

namespace LeadManagement.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateLeadDto, Lead>();
        CreateMap<UpdateLeadDto, Lead>();
        CreateMap<Lead, ReadLeadDto>().ForMember(leadDto => leadDto.ContactFirstName, configExpr =>
        {
            configExpr.MapFrom(lead => lead.ContactFullName.Trim().Split()[0]);
        });
        CreateMap<Lead, ReadAcceptedLeadDto>().ForMember(leadDto => leadDto.FinalPrice, configExpr =>
        {
            configExpr.MapFrom(lead => lead.Price - lead.Discount);
        });
    }
}
