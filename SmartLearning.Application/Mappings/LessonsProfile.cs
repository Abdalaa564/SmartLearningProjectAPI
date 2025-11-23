
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
