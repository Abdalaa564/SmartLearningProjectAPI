
namespace SmartLearning.Application.Mappings
{
    public class UnitProfile :Profile
    {
        public UnitProfile()
        {
            CreateMap<CreateUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();

            CreateMap<Unit, UnitResponseDto>()
                .ForMember(dest => dest.InstructorName,
                    opt => opt.MapFrom(src =>
                        src.Course != null &&
                        src.Course.Instructor != null
                            ? src.Course.Instructor.FullName
                            : string.Empty
                    ));
        }
    }
}
