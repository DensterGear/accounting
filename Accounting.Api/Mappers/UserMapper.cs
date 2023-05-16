using Accounting.Api.Models;
using Accounting.BL;
using AutoMapper;

namespace Accounting.Api.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        ForwardMapping();
        BackwardMapping();
    }

    private void BackwardMapping()
    {
        CreateMap<Domain.User, UserViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => MapGender(src.Gender)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
    }

    private static Gender MapGender(Domain.Gender gender) => gender switch
    {
        Domain.Gender.Male => Gender.Male,
        Domain.Gender.Female => Gender.Female,
        _ => throw new ArgumentOutOfRangeException(nameof(gender))
    };

    private void ForwardMapping()
    {
        CreateMap<UserViewModel, Domain.User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => MapBackwardGender(src.Gender)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
    }

    private static Domain.Gender MapBackwardGender(Gender gender) => gender switch
    {
        Gender.Male => Domain.Gender.Male,
        Gender.Female => Domain.Gender.Female,
        _ => throw new ArgumentOutOfRangeException(nameof(gender))
    };
}