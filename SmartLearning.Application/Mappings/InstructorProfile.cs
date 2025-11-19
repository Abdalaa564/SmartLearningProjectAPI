using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Mappings
{
    public class InstructorProfile:Profile
    {
        public InstructorProfile()
        {
            CreateMap<ApplicationUser, CreateInstructorDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.UserName));

            CreateMap<UpdateInstructorDto, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // لا نغير Id
        }
    }
}
