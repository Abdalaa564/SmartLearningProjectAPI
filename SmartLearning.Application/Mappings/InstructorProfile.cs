
using SmartLearning.Application.DTOs.InstructorDto;
using SmartLearning.Application.DTOs.Instructors;

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile : Profile
    {
        public InstructorProfile()
        {
            CreateMap<CreateInstructorDto, Instructor>();

            CreateMap<UpdateInstructorDto, Instructor>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Instructor, InstructorResponseDto>()
                .ForMember(dest => dest.Email,
                           opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
