using SmartLearning.Application.DTOs.QuizDto;
using SmartLearning.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Mappings
{
   public class MappingProfiles : Profile
    {
		public MappingProfiles()
		{

			CreateMap<Quiz, QuizDetailsDto>()
				   .ForMember(dest => dest.Lesson_Name,
					   opt => opt.MapFrom(src => src.Lesson.Lesson_Name))
				   .ForMember(dest => dest.Questions,
					   opt => opt.MapFrom(src => src.Questions));

			CreateMap<CreateQuizDto, Quiz>();
			CreateMap<UpdateQuizDto, Quiz>();
			CreateMap<Quiz, StartQuizDto>()
				.ForMember(dest => dest.StartTime,
					opt => opt.MapFrom(src => DateTime.Now))
				.ForMember(dest => dest.EndTime,
					opt => opt.MapFrom(src => DateTime.Now.AddMinutes(src.Duration)));

			// Question Mappings
			CreateMap<Questions, QuestionDto>()
				.ForMember(dest => dest.Choices,
					opt => opt.MapFrom(src => src.Choices));

			CreateMap<CreateQuestionDto, Questions>().ForMember(dest => dest.Choices, opt => opt.Ignore()); 
			

			// Choice Mappings
			CreateMap<Choice, ChoiceDto>();
			CreateMap<CreateChoiceDto, Choice>();

			// StudentAnswer Mappings
			CreateMap<SubmitAnswerDto, StudentAnswer>();

			CreateMap<StudentAnswer, StudentAnswerResultDto>()
				.ForMember(dest => dest.Question_Text,
					opt => opt.MapFrom(src => src.Questions.Question_Text))
				.ForMember(dest => dest.Choice_Text,
					opt => opt.MapFrom(src => src.Choice.ChoiceText))
				.ForMember(dest => dest.Grade_Point,
					opt => opt.MapFrom(src => src.Questions.Grade_Point))
				.ForMember(dest => dest.CorrectAnswer,
					opt => opt.MapFrom(src => src.Questions.Choices.FirstOrDefault(c => c.IsCorrect).ChoiceText));
            CreateMap<Grades, StudentGradeDto>()
                        .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.Quize_Id))
                        .ForMember(dest => dest.QuizName, opt => opt.MapFrom(src => src.Quiz.Quiz_Name))
                        .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Crs_Name))
                        .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                        .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.UserName));

            CreateMap<CourseRatingDto, CourseRating>();
			CreateMap<InstructorRatingDto, InstructorRating>();
		}
	}
}
