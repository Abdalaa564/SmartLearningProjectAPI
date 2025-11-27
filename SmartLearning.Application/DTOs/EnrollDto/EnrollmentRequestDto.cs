
namespace SmartLearning.Application.DTOs.EnrollDto
{
    public class EnrollmentRequestDto
    {

        [Required]
        public int StudentId { get; set; }   

        [Required]
        public int CourseId { get; set; }    

        public string? TransactionId { get; set; }   


}
}
