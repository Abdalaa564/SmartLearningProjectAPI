using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.LessonDto
{
    public class LessonResponseDto
    {
        public int Lesson_Id { get; set; }
        public int Unit_Id { get; set; }
        public string Lesson_Name { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
    }
}
