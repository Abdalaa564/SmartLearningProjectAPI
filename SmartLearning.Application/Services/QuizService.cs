using Microsoft.EntityFrameworkCore;
using SmartLearning.Application.DTOs.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services
{
   public class QuizService:IQuizService
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<QuizDetailsDto?> GetQuizByIdAsync(int quizId)
		{
			var quizzes = await _unitOfWork.Repository<Quiz>()
				.FindAsync(
					predicate: q => q.Quiz_Id == quizId,
					includeFunc: q => q
						.Include(x => x.Lesson)
						.Include(x => x.Questions)
							.ThenInclude(qu => qu.Choices)
				);

			var quiz = quizzes.FirstOrDefault();

			if (quiz == null)
				return null;

			return _mapper.Map<QuizDetailsDto>(quiz);
		}

		public async Task<StartQuizDto?> StartQuizAsync(int quizId)
		{
			var quizzes = await _unitOfWork.Repository<Quiz>()
				.FindAsync(
					predicate: q => q.Quiz_Id == quizId,
					includeFunc: q => q
						.Include(x => x.Questions)
							.ThenInclude(qu => qu.Choices)
				);

			var quiz = quizzes.FirstOrDefault();

			if (quiz == null)
				return null;

			return _mapper.Map<StartQuizDto>(quiz);
		}

		public async Task<QuizDetailsDto> CreateQuizAsync(CreateQuizDto quizDto)
		{
			var quiz = _mapper.Map<Quiz>(quizDto);

			await _unitOfWork.Repository<Quiz>().AddAsync(quiz);
			await _unitOfWork.CompleteAsync();

			return _mapper.Map<QuizDetailsDto>(quiz);
		}

		public async Task<bool> UpdateQuizAsync(UpdateQuizDto quizDto)
		{
			var quiz = await _unitOfWork.Repository<Quiz>()
				.GetByIdAsync(quizDto.Quiz_Id);

			if (quiz == null)
				return false;

			quiz.Quiz_Name = quizDto.Quiz_Name;
			quiz.TotalMarks = quizDto.TotalMarks;
			quiz.Duration = quizDto.Duration;

			_unitOfWork.Repository<Quiz>().Update(quiz);
			await _unitOfWork.CompleteAsync();

			return true;
		}

		public async Task<bool> DeleteQuizAsync(int quizId)
		{
			var quiz = await _unitOfWork.Repository<Quiz>()
				.GetByIdAsync(quizId);

			if (quiz == null)
				return false;

			_unitOfWork.Repository<Quiz>().Remove(quiz);
			await _unitOfWork.CompleteAsync();

			return true;
		}

		public async Task<QuestionDto> AddQuestionAsync(CreateQuestionDto questionDto)
		{
			var question = _mapper.Map<Questions>(questionDto);

			await _unitOfWork.Repository<Questions>().AddAsync(question);
			await _unitOfWork.CompleteAsync();

			// إضافة الاختيارات
			foreach (var choiceDto in questionDto.Choices)
			{
				var choice = _mapper.Map<Choice>(choiceDto);
				choice.QuestionId = question.Question_Id;
				await _unitOfWork.Repository<Choice>().AddAsync(choice);
			}

			await _unitOfWork.CompleteAsync();

			// جلب السؤال مع الاختيارات
			var questions = await _unitOfWork.Repository<Questions>()
				.FindAsync(
					predicate: q => q.Question_Id == question.Question_Id,
					includeFunc: q => q.Include(x => x.Choices)
				);

			var result = questions.FirstOrDefault();

			return _mapper.Map<QuestionDto>(result);
		}

		public async Task<bool> DeleteQuestionAsync(int questionId)
		{
			var question = await _unitOfWork.Repository<Questions>()
				.GetByIdAsync(questionId);

			if (question == null)
				return false;

			_unitOfWork.Repository<Questions>().Remove(question);
			await _unitOfWork.CompleteAsync();

			return true;
		}

		public async Task<bool> SubmitAnswerAsync(string userId, SubmitAnswerDto answerDto)
		{
			// التحقق من وجود السؤال
			var question = await _unitOfWork.Repository<Questions>()
				.GetByIdAsync(answerDto.Question_Id);

			if (question == null)
				return false;

			// التحقق من وجود الاختيار
			var choice = await _unitOfWork.Repository<Choice>()
				.GetByIdAsync(answerDto.Choice_Id);

			if (choice == null)
				return false;

			// التحقق من عدم وجود إجابة سابقة لنفس السؤال
			var existingAnswers = await _unitOfWork.Repository<StudentAnswer>()
				.FindAsync(predicate: sa =>
					sa.User_Id == userId &&
					sa.Quiz_Id == answerDto.Quiz_Id &&
					sa.Question_Id == answerDto.Question_Id
				);

			var existingAnswer = existingAnswers.FirstOrDefault();

			if (existingAnswer != null)
			{
				// تحديث الإجابة
				existingAnswer.Choice_Id = answerDto.Choice_Id;
				existingAnswer.Is_Correct = choice.IsCorrect;
				_unitOfWork.Repository<StudentAnswer>().Update(existingAnswer);
			}
			else
			{
				// إضافة إجابة جديدة
				var studentAnswer = new StudentAnswer
				{
					User_Id = userId,
					Quiz_Id = answerDto.Quiz_Id,
					Question_Id = answerDto.Question_Id,
					Choice_Id = answerDto.Choice_Id,
					Is_Correct = choice.IsCorrect
				};

				await _unitOfWork.Repository<StudentAnswer>().AddAsync(studentAnswer);
			}

			await _unitOfWork.CompleteAsync();
			return true;
		}

		public async Task<QuizResultDto?> GetQuizResultAsync(string userId, int quizId)
		{
			var quizzes = await _unitOfWork.Repository<Quiz>()
				.FindAsync(
					predicate: q => q.Quiz_Id == quizId,
					includeFunc: q => q.Include(x => x.Questions)
				);

			var quiz = quizzes.FirstOrDefault();

			if (quiz == null)
				return null;

			var studentAnswers = await _unitOfWork.Repository<StudentAnswer>()
				.FindAsync(
					predicate: sa => sa.User_Id == userId && sa.Quiz_Id == quizId,
					includeFunc: sa => sa
						.Include(x => x.Questions)
						.Include(x => x.Choice)
				);

			if (!studentAnswers.Any())
				return null;

			var correctAnswers = studentAnswers.Count(sa => sa.Is_Correct);
			var obtainedMarks = studentAnswers
				.Where(sa => sa.Is_Correct)
				.Sum(sa => sa.Questions.Grade_Point);

			var result = new QuizResultDto
			{
				Quiz_Id = quiz.Quiz_Id,
				Quiz_Name = quiz.Quiz_Name,
				TotalMarks = quiz.TotalMarks,
				ObtainedMarks = obtainedMarks,
				Percentage = quiz.TotalMarks > 0 ? (double)obtainedMarks / quiz.TotalMarks * 100 : 0,
				CorrectAnswers = correctAnswers,
				TotalQuestions = quiz.Questions.Count,
				Answers = _mapper.Map<List<StudentAnswerResultDto>>(studentAnswers.ToList())
			};

			return result;
		}

		public async Task<List<QuizDetailsDto>> GetQuizzesByLessonIdAsync(int lessonId)
		{
			var quizzes = await _unitOfWork.Repository<Quiz>()
				.FindAsync(
					predicate: q => q.Lesson_Id == lessonId,
					includeFunc: q => q
						.Include(x => x.Lesson)
						.Include(x => x.Questions)
							.ThenInclude(qu => qu.Choices)
				);

			return _mapper.Map<List<QuizDetailsDto>>(quizzes.ToList());
		}
	}
}
