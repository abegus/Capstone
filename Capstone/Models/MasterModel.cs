namespace Capstone.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MasterModel : DbContext
    {
        public MasterModel()
            : base("name=ClassModel")
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.Teaches)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.ClassQuizs)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Class>()
                .HasMany(e => e.Teaches)
                .WithRequired(e => e.Class)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClassQuiz>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.ClassQuiz)
                .HasForeignKey(e => new { e.QuizId, e.ClassId })
                .WillCascadeOnDelete(false);

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
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Quiz>()
                .HasMany(e => e.ClassQuizs)
                .WithRequired(e => e.Quiz)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Answers)
                .WithRequired(e => e.Student)
                .WillCascadeOnDelete(false);
        }
    }
}
