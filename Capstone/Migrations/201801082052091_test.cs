namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Correct = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Notes = c.String(),
                        QuestionId = c.String(nullable: false, maxLength: 128),
                        AttemptId = c.String(nullable: false, maxLength: 128),
                        QuizAttempt_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.QuizAttempt", t => t.QuizAttempt_Id, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.QuizAttempt_Id);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Picture = c.Binary(storeType: "image"),
                        Text = c.String(),
                        Answer = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        StandardId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.CoreStandard", t => t.StandardId)
                .Index(t => t.StandardId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Quiz",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        StandardId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoreStandard", t => t.StandardId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.StandardId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ClassQuiz",
                c => new
                    {
                        QuizId = c.String(nullable: false, maxLength: 128),
                        ClassId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => new { t.QuizId, t.ClassId })
                .ForeignKey("dbo.Class", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Quiz", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        SchoolName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        First = c.String(nullable: false, maxLength: 50),
                        Last = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 50),
                        ClassId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.QuizAttempt",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        date = c.DateTime(nullable: false),
                        numCorrect = c.Int(nullable: false),
                        toalQuestions = c.Int(nullable: false),
                        QuizId = c.String(nullable: false, maxLength: 128),
                        ClassId = c.String(nullable: false, maxLength: 128),
                        StudentId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Student", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.ClassQuiz", t => new { t.QuizId, t.ClassId }, cascadeDelete: true)
                .Index(t => new { t.QuizId, t.ClassId })
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Teaches",
                c => new
                    {
                        ClassId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClassId, t.UserId })
                .ForeignKey("dbo.Class", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ClassId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CoreStandard",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 50),
                        Grade = c.String(nullable: false, maxLength: 10, fixedLength: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Table",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionQuizs",
                c => new
                    {
                        Question_Id = c.String(nullable: false, maxLength: 128),
                        Quiz_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.Quiz_Id })
                .ForeignKey("dbo.Question", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.Quiz", t => t.Quiz_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.Quiz_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionQuizs", "Quiz_Id", "dbo.Quiz");
            DropForeignKey("dbo.QuestionQuizs", "Question_Id", "dbo.Question");
            DropForeignKey("dbo.Teaches", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Quiz", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Quiz", "StandardId", "dbo.CoreStandard");
            DropForeignKey("dbo.Question", "StandardId", "dbo.CoreStandard");
            DropForeignKey("dbo.ClassQuiz", "QuizId", "dbo.Quiz");
            DropForeignKey("dbo.QuizAttempt", new[] { "QuizId", "ClassId" }, "dbo.ClassQuiz");
            DropForeignKey("dbo.Teaches", "ClassId", "dbo.Class");
            DropForeignKey("dbo.QuizAttempt", "StudentId", "dbo.Student");
            DropForeignKey("dbo.Answer", "QuizAttempt_Id", "dbo.QuizAttempt");
            DropForeignKey("dbo.Student", "ClassId", "dbo.Class");
            DropForeignKey("dbo.ClassQuiz", "ClassId", "dbo.Class");
            DropForeignKey("dbo.Question", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answer", "QuestionId", "dbo.Question");
            DropIndex("dbo.QuestionQuizs", new[] { "Quiz_Id" });
            DropIndex("dbo.QuestionQuizs", new[] { "Question_Id" });
            DropIndex("dbo.Teaches", new[] { "UserId" });
            DropIndex("dbo.Teaches", new[] { "ClassId" });
            DropIndex("dbo.QuizAttempt", new[] { "StudentId" });
            DropIndex("dbo.QuizAttempt", new[] { "QuizId", "ClassId" });
            DropIndex("dbo.Student", new[] { "ClassId" });
            DropIndex("dbo.ClassQuiz", new[] { "ClassId" });
            DropIndex("dbo.ClassQuiz", new[] { "QuizId" });
            DropIndex("dbo.Quiz", new[] { "UserId" });
            DropIndex("dbo.Quiz", new[] { "StandardId" });
            DropIndex("dbo.Question", new[] { "UserId" });
            DropIndex("dbo.Question", new[] { "StandardId" });
            DropIndex("dbo.Answer", new[] { "QuizAttempt_Id" });
            DropIndex("dbo.Answer", new[] { "QuestionId" });
            DropTable("dbo.QuestionQuizs");
            DropTable("dbo.Table");
            DropTable("dbo.CoreStandard");
            DropTable("dbo.Teaches");
            DropTable("dbo.QuizAttempt");
            DropTable("dbo.Student");
            DropTable("dbo.Class");
            DropTable("dbo.ClassQuiz");
            DropTable("dbo.Quiz");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
