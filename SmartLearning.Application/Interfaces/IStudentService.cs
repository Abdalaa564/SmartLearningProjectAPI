using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
    public interface IStudentService
    {
        Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto registerDto);
        Task<StudentProfileDto> GetStudentProfileAsync(string userId);

        Task<StudentProfileDto> UpdateStudentAsync(string userId, StudentUpdateDto updateDto);

    }
}
