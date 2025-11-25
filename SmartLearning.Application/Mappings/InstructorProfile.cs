

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            // Create ? Entity
            CreateMap<CreateInstructorDto, Instructor>();

            // Update ? Entity (partial update)
            CreateMap<UpdateInstructorDto, Instructor>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Instructor, InstructorResponseDto>()
                .ForMember(dest => dest.Email,
                           opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
