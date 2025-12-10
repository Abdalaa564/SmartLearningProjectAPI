using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Core.Model
{
   public class InstructorRating
    {
		[Key]
		public int Id { get; set; }

		[Required]
		public int InstructorId { get; set; }
		public Instructor Instructor { get; set; } = null!;

		[Required]
		public string UserId { get; set; } = string.Empty;
		public ApplicationUser User { get; set; } = null!;

		[Range(1, 5)]
		public int RatingValue { get; set; }

		[MaxLength(1000)]
		public string Feedback { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.Now;
	}
}
