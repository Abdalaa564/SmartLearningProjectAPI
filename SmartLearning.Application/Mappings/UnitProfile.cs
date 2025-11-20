using SmartLearning.Application.DTOs.UnitDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
