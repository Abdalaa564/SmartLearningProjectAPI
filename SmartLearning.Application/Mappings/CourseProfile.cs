
using SmartLearning.Application.DTOs.CourseDto;

namespace SmartLearning.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseResponseDto>()
               .ForMember(dest => dest.InstructorName,
                          opt => opt.MapFrom(src => src.Instructor.FullName)); 

            CreateMap<AddCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
        }
    }
}
