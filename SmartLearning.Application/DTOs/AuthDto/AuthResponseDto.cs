using SmartLearning.Application.DTOs.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.AuthDto
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public StudentProfileDto Data { get; set; }

        public TokenResponseDto Token { get; set; }
    }
}
