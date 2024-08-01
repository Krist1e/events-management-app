using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.UseCases.Events.Contracts;

namespace EventsManagementApp.Application.Common.Mappings;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventRequest, Event>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CategoryEnum>(src.Category)));

        CreateMap<CreateEventRequest, Event>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CategoryEnum>(src.Category)));

        CreateMap<Event, EventResponse>()
            .ForCtorParam(nameof(EventResponse.Id), opt => opt.MapFrom(src => src.Id.ToString()))
            .ForCtorParam(nameof(EventResponse.Category), opt => opt.MapFrom(src => src.Category.ToString()));
        
        CreateMap<Event, CreateEventResponse>()
            .ForCtorParam(nameof(CreateEventResponse.Id), opt => opt.MapFrom(src => src.Id.ToString()))
            .ForCtorParam(nameof(CreateEventResponse.Category), opt => opt.MapFrom(src => src.Category.ToString()));
            
        CreateMap<Image, ImageResponse>()
            .ForCtorParam(nameof(ImageResponse.Id), opt => opt.MapFrom(src => src.Id.ToString()))
            .ForCtorParam(nameof(ImageResponse.Url), opt => opt.MapFrom(src => src.ImageUrl));
    }
}