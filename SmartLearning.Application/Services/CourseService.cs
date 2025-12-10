
namespace SmartLearning.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
       

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
           
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllCourseAsync()
        {
            var result = await _unitOfWork.Repository<Course>()
                .GetAllAsync(c => c.Instructor,
                             c => c.Instructor.User);

            return _mapper.Map<IEnumerable<CourseResponseDto>>(result);
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Repository<Course>()
                .GetAllAsync(query =>
                    query
                        .Where(c => c.Crs_Id == id)
                        .Include(c => c.Instructor)
                            .ThenInclude(i => i.User)
                        .Include(c => c.Enrollments)
                            .ThenInclude(e => e.Student)
                                .ThenInclude(s => s.User)
                );

            var entity = result.FirstOrDefault();
            return _mapper.Map<CourseResponseDto>(entity);
        }

        // ADD COURSE
        public async Task<bool> AddCourseAsync(AddCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);

            await _unitOfWork.Repository<Course>().AddAsync(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto, string? uploadedImagePath)
        {
            var courseRepo = _unitOfWork.Repository<Course>();
            var instructorRepo = _unitOfWork.Repository<Instructor>();

            var course = await courseRepo.GetByIdAsync(id);
            if (course == null) return false;

            course.Crs_Name = dto.Crs_Name;
            course.Crs_Description = dto.Crs_Description;
            course.Price = dto.Price;
            course.InstructorId = dto.InstructorId;

            if (!string.IsNullOrWhiteSpace(uploadedImagePath))
            {
                course.ImageUrl = uploadedImagePath;
            }
            else if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                course.ImageUrl = dto.ImageUrl;
            }

            courseRepo.Update(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        // DELETE COURSE
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null) return false;

            _unitOfWork.Repository<Course>().Remove(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public async Task<int> GetCoursesCountAsync()
        {
            var courses = await _unitOfWork.Repository<Course>().GetAllAsync();
            return courses.Count;
        }

    }
}