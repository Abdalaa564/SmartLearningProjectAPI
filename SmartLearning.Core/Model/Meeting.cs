
namespace SmartLearning.Core.Model
{
    public class Meeting // Event
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartsAt { get; set; }

        [Required, MaxLength(500)]
        public string JoinLink { get; set; } = string.Empty;

        [Required]  // FK → ApplicationUser
        public string CreatedBy { get; set; } = string.Empty;
        
        public ApplicationUser User { get; set; } = null!;
    }
}
