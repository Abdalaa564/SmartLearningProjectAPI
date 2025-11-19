
using SmartLearning.Core.Model;

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

        public DbSet<Course> Courses { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Course ↔ Instructor (User) (1 → M)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.User)
                .WithMany(u => u.Courses)
                .HasForeignKey(c => c.User_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Course ↔ Unit (1 → M)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Course)
                .WithMany(c => c.Units)
                .HasForeignKey(u => u.Crs_Id);

            //Course ↔ Enrollment (Many-to-Many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.Crs_Id)
                .OnDelete(DeleteBehavior.Cascade);

            //Course ↔ Enrollment (Many-to-Many)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Unit ↔ Lessons (1 → M)
            modelBuilder.Entity<Lessons>()
                .HasOne(l => l.Unit)
                .WithMany(u => u.Lessons)
                .HasForeignKey(l => l.Unit_Id);

            // Lessons ↔ Quiz (1 → M)
            modelBuilder.Entity<Quiz>()
                .HasOne(q => q.Lesson)
                .WithMany(l => l.Quizzes)
                .HasForeignKey(q => q.Lesson_Id);

            // Lessons ↔ Resource (1 → M)
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Lesson)
                .WithMany(l => l.Resources)
                .HasForeignKey(r => r.Lesson_Id);

            // Lessons ↔ Rating (1 → M)
            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.Lesson_Id, r.User_Id }); // Composite Key

            // Lessons ↔ Rating (1 → M)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Lesson)
                .WithMany(l => l.Ratings)
                .HasForeignKey(r => r.Lesson_Id);

            // Lessons ↔ Rating (1 → M)
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.User_Id);

            // Quiz ↔ Questions (1 → M)
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.Quiz_Id);


            modelBuilder.Entity<Questions>()
                .Property(q => q.Question_Type)
                .HasConversion<string>();

            // Questions ↔ Choices (1 → M)
            modelBuilder.Entity<Choice>()
                .HasOne(c => c.Question)
                .WithMany(q => q.Choices)
                .HasForeignKey(c => c.QuestionId);

            // Questions ↔ Grades (1 → M)
            modelBuilder.Entity<Grades>()
                .HasOne(g => g.Quiz)
                .WithMany(q => q.Grades)
                .HasForeignKey(g => g.Quize_Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grades>()
                .HasOne(g => g.Student)
                .WithMany(u => u.Grades)
                .HasForeignKey(g => g.Std_Id)
                .OnDelete(DeleteBehavior.Cascade);


            // Questions ↔ StudentAnswer (1 → M)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Choice)
                .WithMany()
                .HasForeignKey(sa => sa.Choice_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Questions ↔ StudentAnswer (1 → M)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Quiz)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.Quiz_Id)
                .OnDelete(DeleteBehavior.NoAction);

            // Questions ↔ StudentAnswer (1 → M)
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.User)
                .WithMany()
                .HasForeignKey(sa => sa.User_Id)
                .OnDelete(DeleteBehavior.NoAction);


            // Attendance ↔ Lessons (M → M)
            modelBuilder.Entity<Attendance>()
                .HasMany(a => a.Lessons)
                .WithMany(l => l.Attendances)
                .UsingEntity<Dictionary<string, object>>(
                    "AttendanceLessons",
                    j => j.HasOne<Lessons>().WithMany().HasForeignKey("Lesson_Id"),
                    j => j.HasOne<Attendance>().WithMany().HasForeignKey("Attendance_Id"),
                    j =>
                    {
                        j.HasKey("Attendance_Id", "Lesson_Id");
                        j.ToTable("AttendanceLessons");
                    }
                );

            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.Attendance_Id);

            // Attendance ↔ User (1 → M)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.User_Id)
                .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
