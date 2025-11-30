
namespace SmartLearning.Application.Mappings
{
    public class LessonsProfile:Profile
    {
        public LessonsProfile()
        {
            CreateMap<CreateLessonDto, Lessons>();
            CreateMap<UpdateLessonDto, Lessons>();

            CreateMap<Lessons, LessonResponseDto>()
                .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.Resources));

            CreateMap<Resource, ResourceResponseDto>();

            CreateMap<Lessons, LessonDetailsDto>();
        }
    }
}
