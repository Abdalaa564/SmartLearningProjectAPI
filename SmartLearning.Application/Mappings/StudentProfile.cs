
using SmartLearning.Application.DTOs.AuthDto;
using SmartLearning.Application.DTOs.StudentDto;

namespace SmartLearning.Application.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // Student to StudentProfileDto
            CreateMap<Student, StudentProfileDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            // StudentRegisterDto to Student
            CreateMap<RegisterStudentDto, Student>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
                //.ForMember(dest => dest.EnrollmentNumber, opt => opt.Ignore());

            // StudentUpdateDto to Student
            CreateMap<StudentUpdateDto, Student>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

           
        }
    }
}
