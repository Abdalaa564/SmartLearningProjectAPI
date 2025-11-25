using SmartLearning.Application.DTOs.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
   public interface IQuizService
    {
		Task<QuizDetailsDto?> GetQuizByIdAsync(int quizId);
		Task<StartQuizDto?> StartQuizAsync(int quizId);
		Task<QuizDetailsDto> CreateQuizAsync(CreateQuizDto quizDto);
		Task<bool> UpdateQuizAsync(UpdateQuizDto quizDto);
		Task<bool> DeleteQuizAsync(int quizId);
		Task<QuestionDto> AddQuestionAsync(CreateQuestionDto questionDto);
		Task<bool> DeleteQuestionAsync(int questionId);
		Task<bool> SubmitAnswerAsync(string userId, SubmitAnswerDto answerDto);
		Task<QuizResultDto?> GetQuizResultAsync(string userId, int quizId);
		Task<List<QuizDetailsDto>> GetQuizzesByLessonIdAsync(int lessonId);
	}
}
