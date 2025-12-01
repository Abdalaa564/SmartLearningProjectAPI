

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

            CreateMap<AddCourseDto, Course>();

            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.Crs_Name,
                    opt => opt.MapFrom(src => src.Crs_Name))
                .ForMember(dest => dest.Crs_Description,
                    opt => opt.MapFrom(src => src.Crs_Description))
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.InstructorId,
                    opt => opt.MapFrom(src => src.InstructorId))
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => src.ImageUrl));
        }
    }
}
