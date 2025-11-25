

namespace SmartLearning.Application.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MeetingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MeetingResponseDto> CreateMeetingAsync(string userId, CreateMeetingDto dto)
        {
            var meeting = new Meeting
            {
                CreatedBy = userId,
                Description = dto.Description,
                StartsAt = dto.StartsAt,
                JoinLink = $"https://localhost:4200/meeting/{Guid.NewGuid()}"
            };

            await _unitOfWork.Repository<Meeting>().AddAsync(meeting);
            await _unitOfWork.CompleteAsync();

            meeting.User = (await _unitOfWork.Repository<ApplicationUser>()
                .FindAsync(u => u.Id == userId))
                .FirstOrDefault();

            return _mapper.Map<MeetingResponseDto>(meeting);
        }
        public async Task<List<MeetingResponseDto>> GetAllMeetingsAsync(string? userId)
        {
            var meetings = string.IsNullOrEmpty(userId)
                ? await _unitOfWork.Repository<Meeting>().GetAllAsync(m => m.User)
                : await _unitOfWork.Repository<Meeting>().FindAsync(m => m.CreatedBy == userId, m => m.User);

            return _mapper.Map<List<MeetingResponseDto>>(meetings);
        }
        public async Task<MeetingResponseDto?> GetMeetingByIdAsync(Guid id)
        {
            var meetings = await _unitOfWork.Repository<Meeting>().FindAsync(m => m.Id == id, m => m.User);
            var meeting = meetings.FirstOrDefault();
            return _mapper.Map<MeetingResponseDto?>(meeting);
        }

        public async Task<bool> DeleteMeetingAsync(Guid id, string userId)
        {
            var meetings = await _unitOfWork.Repository<Meeting>().FindAsync(m => m.Id == id && m.CreatedBy == userId);
            var meeting = meetings.FirstOrDefault();

            if (meeting == null) return false;

            _unitOfWork.Repository<Meeting>().Remove(meeting);
            await _unitOfWork.CompleteAsync();
            return true;
        }



    }
}
