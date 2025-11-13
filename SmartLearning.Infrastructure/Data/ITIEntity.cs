
namespace SmartLearning.Infrastructure.Data
{
    public class ITIEntity : IdentityDbContext<ApplicationUser>
    {
        public ITIEntity()
        {

        }
        public ITIEntity(DbContextOptions options) : base(options)
        {
        }
        
    }
}
