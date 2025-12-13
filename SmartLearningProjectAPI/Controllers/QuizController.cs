
namespace SmartLearningProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class QuizController : ControllerBase
	{
		private readonly IQuizService _quizService;
        private readonly IStudentService _studentService;
        public QuizController(IQuizService quizService, IStudentService studentService)
		{
			_quizService = quizService;
			_studentService = studentService;
		}

        // GET: api/Quiz/{id}
        [Authorize(Roles = "Instructor,Admin,Student")]
        [HttpGet("{id}")]
		public async Task<IActionResult> GetQuizById(int id)
		{
			var quiz = await _quizService.GetQuizByIdAsync(id);

			if (quiz == null)
				return NotFound(new { message = "Quiz not found" });

			return Ok(quiz);
		}

        // GET: api/Quiz/lesson/{lessonId}
        [Authorize(Roles = "Instructor,Admin,Student")]
        [HttpGet("lesson/{lessonId}")]
		public async Task<IActionResult> GetQuizzesByLessonId(int lessonId)
		{
			var quizzes = await _quizService.GetQuizzesByLessonIdAsync(lessonId);
			return Ok(quizzes);
		}

        // POST: api/Quiz/start/{quizId}
        [Authorize]
        [HttpPost("start/{quizId}")]
		public async Task<IActionResult> StartQuiz(int quizId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var quiz = await _quizService.StartQuizAsync(quizId, userId);

			if (quiz == null)
				return BadRequest(new { message = "You have already taken this quiz or quiz not found." });

			return Ok(quiz);

		}

		// POST: api/Quiz
		[HttpPost]
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDto quizDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var quiz = await _quizService.CreateQuizAsync(quizDto);
			return CreatedAtAction(nameof(GetQuizById), new { id = quiz.Quiz_Id }, quiz);
		}

		// PUT: api/Quiz
		[HttpPut]
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizDto quizDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _quizService.UpdateQuizAsync(quizDto);

			if (!result)
				return NotFound(new { message = "Quiz not found" });

			return Ok(new { message = "Quiz updated successfully" });
		}

		// DELETE: api/Quiz/{id}
		[HttpDelete("{id}")]
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> DeleteQuiz(int id)
		{
			var result = await _quizService.DeleteQuizAsync(id);

			if (!result)
				return NotFound(new { message = "Quiz not found" });

			return Ok(new { message = "Quiz deleted successfully" });
		}

		// POST: api/Quiz/question
		[HttpPost("question")]
		[Authorize(Roles = "Instructor")]
		public async Task<IActionResult> AddQuestion([FromBody] CreateQuestionDto questionDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var question = await _quizService.AddQuestionAsync(questionDto);
			return Ok(question);
		}

		// DELETE: api/Quiz/question/{id}
        [Authorize(Roles = "Instructor,Admin")]
		[HttpDelete("question/{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
		{
			var result = await _quizService.DeleteQuestionAsync(id);

			if (!result)
				return NotFound(new { message = "Question not found" });

			return Ok(new { message = "Question deleted successfully" });
		}

		// POST: api/Quiz/submit-answer
		[Authorize(Roles = "Student")]
		[HttpPost("submit-answer")]
		public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto answerDto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await _quizService.SubmitAnswerAsync(userId, answerDto);

			if (!result)
				return BadRequest(new { message = "Failed to submit answer" });

			return Ok(new { message = "Answer submitted successfully" });
		}

		// GET: api/Quiz/result/{quizId}
		[Authorize]
		[HttpGet("result/{quizId}")]
		public async Task<IActionResult> GetQuizResult(int quizId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await _quizService.GetQuizResultAsync(userId, quizId);

			if (result == null)
				return NotFound(new { message = "Quiz not found or no answers submitted" });

			return Ok(result);
		}

		[Authorize]
		[HttpGet("StudentGrades")]
		public async Task<IActionResult> GetStudentGrades()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var result = await _quizService.GetStudentGradesAsync(userId);
			return Ok(result);
		}

		// GET: api/Quiz/all
		[Authorize]
		[HttpGet("all")]
		public async Task<IActionResult> GetAllQuizzes()
		{
			var quizzes = await _quizService.GetAllQuizzesAsync();

			if (quizzes == null || !quizzes.Any())
				return NotFound(new { message = "No quizzes found." });

			return Ok(quizzes);
		}
        [HttpGet("GetAllGrades")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> StudentGradesAsync()
        {
            var result = await _quizService.StudentGradesAsync();
            return Ok(result);
        }
    }
}
