
namespace SmartLearning.Core.Model
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Grades> Grades { get; set; } = new List<Grades>();

     
    }
}