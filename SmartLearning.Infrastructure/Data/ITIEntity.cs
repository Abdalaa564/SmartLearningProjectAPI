
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
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Instructor ↔ User (Identity User) (1 → 1)
            modelBuilder.Entity<Instructor>()
                 .HasOne(i => i.User)
                 .WithMany()
                 .HasForeignKey(i => i.UserId)
                 .OnDelete(DeleteBehavior.Cascade);



            // Instructor ↔ Course (1 → M)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);


            // Course ↔ Unit (1 → M)
            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Course)
                .WithMany(c => c.Units)
                .HasForeignKey(u => u.Crs_Id);

            //Course ↔ Enrollment (Many-to-1)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.Crs_Id)
                .OnDelete(DeleteBehavior.Cascade);

            //Course ↔ Enrollment (Many-to-1)
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

            // Lessons ↔ Rating (M → M)
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

            // Convert enum to string in DB
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
                .OnDelete(DeleteBehavior.NoAction);

            // Grades ↔ Student(User) 1 → M
            modelBuilder.Entity<Grades>()
                .HasOne(g => g.Student)
                .WithMany(u => u.Grades)
                .HasForeignKey(g => g.Std_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Grades ↔ Course 1 → M
            modelBuilder.Entity<Grades>()
                .HasOne(g => g.Course)
                .WithMany(c => c.Grades)
                .HasForeignKey(g => g.Course_Id)
                .OnDelete(DeleteBehavior.Restrict);


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


            // Attendance ↔ Lessons (1 → M)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Lesson)
                .WithMany(l => l.Attendances)
                .HasForeignKey(a => a.Lesson_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance (PK)
            modelBuilder.Entity<Attendance>()
                .HasKey(a => a.Attendance_Id);

            // Meeting ↔ User (Identity User) (M → 1)
            modelBuilder.Entity<Meeting>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.CreatedBy)
                .OnDelete(DeleteBehavior.Cascade);

            //for Attendance 
            // Attendance ↔ Lessons (1 → M)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Lesson)
                .WithMany(l => l.Attendances)
                .HasForeignKey(a => a.Lesson_Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Attendance ↔ Student (1 → M)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
