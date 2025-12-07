

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
           // Entity → ResponseDto
            CreateMap<Instructor, InstructorResponseDto>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.NumberOfStudents,
                    opt => opt.MapFrom(src =>
                        src.Courses
                           .SelectMany(c => c.Enrollments)
                           .Select(e => e.StudentId)
                           .Distinct()
                           .Count()
                    ));
            // باقي الحقول (CvUrl, Specialization, UniversityName, About, Status)
            // هتتبعت أوتوماتيك لأن الأسماء متطابقة.

            // CreateInstructorDto → Instructor (Admin create)
            CreateMap<CreateInstructorDto, Instructor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore()); // بيحدد في السيرفس

            // UpdateInstructorDto → Instructor
            CreateMap<UpdateInstructorDto, Instructor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // RegisterInstructorDto → Instructor (Public register)
            CreateMap<RegisterInstructorDto, Instructor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.NumberOfStudents, opt => opt.Ignore())
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.Courses, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => InstructorStatus.Pending));
        }
    }
}
