using OpenAI;
using OpenAI.Chat;
using SmartLearning.Application.DTOs.QuizDto;




namespace SmartLearning.Application.Services
{
	public class QuizService : IQuizService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IConfiguration config;

		public QuizService(IUnitOfWork unitOfWork, IMapper mapper,IConfiguration config)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			this.config = config;
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

		public async Task<StartQuizDto?> StartQuizAsync(int quizId, string userId)
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
			var existingAnswers = await _unitOfWork.Repository<StudentAnswer>()
			.FindAsync(sa => sa.User_Id == userId && sa.Quiz_Id == quizId);

			if (existingAnswers.Any())
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
					Is_Correct = choice.IsCorrect,
					Questions = question
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
					includeFunc: q => q.Include(x => x.Questions).Include(x => x.Lesson).ThenInclude(c => c.Unit)
				);

			var quiz = quizzes.FirstOrDefault();

			if (quiz == null)
				return null;

			var studentAnswers = await _unitOfWork.Repository<StudentAnswer>()
				.FindAsync(
						sa => sa.User_Id == userId && sa.Quiz_Id == quizId,
						includeFunc: sa => sa
							.Include(x => x.Questions)
								.ThenInclude(q => q.Choices)
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

			var courseId = quiz.Lesson.Unit.Crs_Id;
			var existingGrade = await _unitOfWork.Repository<Grades>()
	   .FindAsync(g => g.Std_Id == userId && g.Quize_Id == quizId);

			var grade = existingGrade.FirstOrDefault();

			if (grade == null)
			{
				grade = new Grades
				{
					Std_Id = userId,
					Quize_Id = quizId,
					Course_Id = courseId,
					Value = (decimal)result.Percentage
				};

				await _unitOfWork.Repository<Grades>().AddAsync(grade);
			}
			else
			{
				grade.Value = (decimal)result.Percentage;
				_unitOfWork.Repository<Grades>().Update(grade);
			}

			await _unitOfWork.CompleteAsync();
			result.AiReport = await GenerateAiReportAsync(result);

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

		public async Task<List<StudentGradeDto>> GetStudentGradesAsync(string userId)
		{
			var grades = await _unitOfWork.Repository<Grades>()
				.FindAsync(
					g => g.Std_Id == userId,
					includeFunc: q => q
						.Include(x => x.Quiz)
						.Include(x => x.Course)
				);
			return _mapper.Map<List<StudentGradeDto>>(grades);
		}

		public async Task<List<QuizDetailsDto>> GetAllQuizzesAsync()
		{

			var quizzes = await _unitOfWork.Repository<Quiz>()
				.GetAllAsync(q => q
					.Include(x => x.Lesson)
					.Include(x => x.Questions)
						.ThenInclude(qu => qu.Choices)
				);

			return _mapper.Map<List<QuizDetailsDto>>(quizzes.ToList());
		}
        public async Task<string> GenerateAiReportAsync(QuizResultDto result)
        {
			var prompt = $@"
				Write a smart and detailed report for the student about their exam performance:
	
				Quiz Name: {result.Quiz_Name}
				Total Marks: {result.TotalMarks}
				Marks Obtained: {result.ObtainedMarks}
				Percentage: {result.Percentage}%
				Total Questions: {result.TotalQuestions}
				Correct Answers: {result.CorrectAnswers}
				Wrong Answers: {result.TotalQuestions - result.CorrectAnswers}
	
				Student Answers Details:
				{string.Join("\n", result.Answers.Select(a =>
									$"Question: {a.Question_Text} | Student Answer: {a.Choice_Text} | {(a.Is_Correct ? "Correct" : "Incorrect")}"
								))}
				";


            var client = new ChatClient(
                model: "gpt-4o-mini",
                apiKey: $"{config["OpenAI:ApiKey"]}"
            );


            var userMessage = new UserChatMessage(prompt);


            ChatCompletion response = await client.CompleteChatAsync(userMessage);


            string aiReport = response.Content[0].Text;

            return aiReport;
        }
        public async Task<List<StudentGradeDto>> StudentGradesAsync()
        {
            var grades = await _unitOfWork.Repository<Grades>()
        .GetAllAsync(q => q
            .Include(x => x.Quiz)
            .Include(x => x.Course)
            .Include(x => x.Student) // لو عندك Navigation للطالب
        );

            return _mapper.Map<List<StudentGradeDto>>(grades);
        }

    }
}
