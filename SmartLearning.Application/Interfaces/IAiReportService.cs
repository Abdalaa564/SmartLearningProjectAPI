using SmartLearning.Application.DTOs.QuizDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
   public interface IAiReportService
    {
		Task<string> GenerateQuizReportAsync(QuizResultDto result);
	}
}
