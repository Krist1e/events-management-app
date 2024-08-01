using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.UseCases.Users.Contracts;

namespace EventsManagementApp.Application.Common.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForCtorParam(nameof(UserResponse.Id), opt => opt.MapFrom(src => src.Id.ToString()));

        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));

        CreateMap<LoginRequest, User>();
        
        CreateMap<RegisterRequest, User>();
    }
}