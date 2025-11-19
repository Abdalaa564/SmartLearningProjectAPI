
namespace SmartLearning.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseResponseDTO>()
                .ForMember(dest => dest.InstructorName,
                           opt => opt.MapFrom(src => src.User.UserName));

            
        }
    }
}
