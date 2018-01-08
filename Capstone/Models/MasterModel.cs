namespace Capstone.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MasterModel : DbContext
    {
        public MasterModel()
            : base("name=NewCapstoneModel")
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<ClassQuiz> ClassQuizs { get; set; }
        public virtual DbSet<CoreStandard> CoreStandards { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Quiz> Quizs { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Table> Tables { get; set; }
        public virtual DbSet<Teach> Teaches { get; set; }

        //added QuizAttempt in between (ClassQuiz, Student) <== QuizAttempt <== Answer ==> Question
        //from                         (ClassQuiz, Student) <== Answer ==> Question
        public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

        //added for many to many relationship
        //public virtual DbSet<QuestionQuizs> QuestionQuizs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Teaches)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            //ADDING OWNERSHIP TO QUESTIONS
            modelBuilder.Entity<AspNetUser>()
               .HasMany(e => e.Questions)
               .WithRequired(e => e.AspNetUser)
               .HasForeignKey(e => e.UserId)
               .WillCascadeOnDelete(false);
            //ADDING OWNERSHIP TO QUIZS
            modelBuilder.Entity<AspNetUser>()
               .HasMany(e => e.Quizs)
               .WithRequired(e => e.AspNetUser)
               .HasForeignKey(e => e.UserId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.ClassQuizs)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Teaches)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ClassQuiz>()
                .HasMany(e => e.QuizAttempts)
                .WithRequired(e => e.ClassQuiz)
                .HasForeignKey(e => new { e.QuizId, e.ClassId })
                .WillCascadeOnDelete(true);

            /*modelBuilder.Entity<ClassQuiz>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.ClassQuiz)
                .HasForeignKey(e => new { e.QuizId, e.ClassId })
                .WillCascadeOnDelete(false);*/

            /*modelBuilder.Entity<QuestionQuizs>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.ClassQuiz)
                .HasForeignKey(e => new { e.QuizId, e.ClassId })
                .WillCascadeOnDelete(false);*/

            modelBuilder.Entity<CoreStandard>()
                .Property(e => e.Grade)
                .IsFixedLength();

            modelBuilder.Entity<CoreStandard>()
                .HasMany(e => e.Questions)
                .WithRequired(e => e.CoreStandard)
                .HasForeignKey(e => e.StandardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CoreStandard>()
                .HasMany(e => e.Quizs)
                .WithRequired(e => e.CoreStandard)
                .HasForeignKey(e => e.StandardId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Quiz>()
                .HasMany<Question>(s => s.Questions)
                .WithMany(c => c.Quizs)
                .Map(cs =>
                {
                    cs.MapLeftKey("Quiz_Id");
                    cs.MapRightKey("Question_Id");
                    cs.ToTable("QuestionQuizs");
                });

            modelBuilder.Entity<Question>()
                .HasMany<Quiz>(s => s.Quizs)
                .WithMany(c => c.Questions)
                .Map(cs =>
                {
                    cs.MapLeftKey("Question_Id");
                    cs.MapRightKey("Quiz_Id");
                    cs.ToTable("QuestionQuizs");
                });

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.ClassQuizs)
                .WithRequired(e => e.Quiz)
                .WillCascadeOnDelete(true); //changed from false;

            modelBuilder.Entity<Student>()
                .HasMany(e => e.QuizAttempts)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<QuizAttempt>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.QuizAttempt)
                .WillCascadeOnDelete(true);
        }

    }
}
