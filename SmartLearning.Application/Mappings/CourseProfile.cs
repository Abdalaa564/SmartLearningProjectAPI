

namespace SmartLearning.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseResponseDto>()
           .ForMember(dest => dest.InstructorName,
                      opt => opt.MapFrom(src => src.Instructor.FullName))
           .ForMember(dest => dest.InstructorPhoto,
                      opt => opt.MapFrom(src => src.Instructor.PhotoUrl));
            // لو عندك Course.ImageUrl هتتـmap أوتوماتيك لو نفس الاسم في DTO

            CreateMap<AddCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
        }
    }
}
