
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
        //public DbSet<Instructor> Employees { get; set; }
        //public DbSet<Department> Departments { get; set; }
        //public DbSet<Student> Students { get; set; }
        //public DbSet<Course> Courses { get; set; }
        //public DbSet<InstructorCourse> InstructorCourses { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<InstructorCourse>()
        //        .HasKey(ic => new { ic.InstructorId, ic.CourseId });

        //    modelBuilder.Entity<InstructorCourse>()
        //        .HasOne(ic => ic.Instructor)
        //        .WithMany(i => i.InstructorCourses)
        //        .HasForeignKey(ic => ic.InstructorId);

        //    modelBuilder.Entity<InstructorCourse>()
        //        .HasOne(ic => ic.Course)
        //        .WithMany(c => c.InstructorCourses)
        //        .HasForeignKey(ic => ic.CourseId);
        //}
    }
}
