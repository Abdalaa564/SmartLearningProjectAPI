using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class StudentGradeDto
    {
        public int QuizId { get; set; }
        public string? StudentName { get; set; }
        public string QuizName { get; set; }
        public string CourseName { get; set; }
        public decimal Value { get; set; }
    }
}
