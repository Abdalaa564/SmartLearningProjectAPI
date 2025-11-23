
namespace SmartLearning.Application.Mappings
{
    public class UnitProfile :Profile
    {
        public UnitProfile()
        {
            CreateMap<CreateUnitDto, Unit>();
            CreateMap<UpdateUnitDto, Unit>();
            CreateMap<Unit, UnitResponseDto>();
        }
    }
}
