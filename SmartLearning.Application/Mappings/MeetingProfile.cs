
namespace SmartLearning.Application.Mappings
{
    public class MeetingProfile : Profile
    {
        public MeetingProfile()
        {
            // Mapping from Meeting entity → MeetingResponseDto
            CreateMap<Meeting, MeetingResponseDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            // Mapping from CreateMeetingDto → Meeting entity
            CreateMap<CreateMeetingDto, Meeting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.JoinLink, opt => opt.Ignore());
        }
    }
}
