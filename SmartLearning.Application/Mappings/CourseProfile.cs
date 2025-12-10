

namespace SmartLearning.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            // 🔹 Student → CourseStudentDto
            CreateMap<Student, CourseStudentDto>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.User.Email));
               

            // 🔹 Course → CourseResponseDto
            CreateMap<Course, CourseResponseDto>()
              .ForMember(dest => dest.InstructorName,
                         opt => opt.MapFrom(src => src.Instructor.FullName))
              .ForMember(dest => dest.InstructorPhoto,
                         opt => opt.MapFrom(src => src.Instructor.PhotoUrl))
              // ✅ الطلاب المسجلين
              .ForMember(dest => dest.EnrolledStudents,
                         opt => opt.MapFrom(src => src.Enrollments.Select(e => e.Student)));

            // 🔹 AddCourseDto → Course
            CreateMap<AddCourseDto, Course>();

            // 🔹 UpdateCourseDto → Course
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
