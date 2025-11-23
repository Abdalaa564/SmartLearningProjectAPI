

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            // Create → Entity
            CreateMap<DTOs.CreateInstructorDto, Instructor>();

            // Update → Entity (partial update)
            CreateMap<DTOs.UpdateInstructorDto, Instructor>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            // Entity → Response
            CreateMap<Instructor, InstructorResponseDto>()
                .ForMember(dest => dest.Email,
                           opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
