using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs
{
    public class UpdateInstructorDto
    {
        public string? JobTitle { get; set; }
        public int? NumberOfStudents { get; set; }
        public double? Rating { get; set; }
        public string? PhoneNumber { get; set; }
        public string? YoutubeChannelUrl { get; set; }
    }
}
