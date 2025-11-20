using SmartLearning.Application.DTOs.LessonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Mappings
{
    public class LessonsProfile:Profile
    {
        public LessonsProfile()
        {
            CreateMap<CreateLessonDto, Lessons>();
            CreateMap<UpdateLessonDto, Lessons>();
            CreateMap<Lessons, LessonResponseDto>();
        }
    }
}
