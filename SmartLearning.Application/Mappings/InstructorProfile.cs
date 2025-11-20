
namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            // Input DTO → Entity
            CreateMap<CreateInstructorDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateInstructorDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore());

            // Output Entity → DTO
            CreateMap<ApplicationUser, CreateInstructorDto>();
        }
    }
}
