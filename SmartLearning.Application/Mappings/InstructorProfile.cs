

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            // من Entity → ResponseDto
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

            // من CreateDto → Entity
            CreateMap<CreateInstructorDto, Instructor>()
                .ForMember(dest => dest.NumberOfStudents, opt => opt.Ignore());

            // من UpdateDto → Entity
            CreateMap<UpdateInstructorDto, Instructor>()
                .ForMember(dest => dest.NumberOfStudents, opt => opt.Ignore());
        }
    }
}
