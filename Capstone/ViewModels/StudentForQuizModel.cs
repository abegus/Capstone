using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Capstone.Models;
using System.Collections;
using System.Linq;

namespace Capstone.ViewModels
{

    public class StudentForQuizModel
    {
        private MasterModel db = new MasterModel();

        public Quiz quiz { get; set; }
        public Student student { get; set; }
        public Class cla { get; set; }
        public List<QuizAttempt> attempts { get; set; }

        public StudentForQuizModel(Quiz quiz, Student student, Class cla)
        {
            this.quiz = quiz;
            this.student = student;
            this.cla = cla;

            //var quizAttempts = quiz.ClassQuizs.Where(q => q.ClassId == cla.Id).Select(u => u.QuizAttempts);
            var quizAttempts = (from qa in db.QuizAttempts where qa.StudentId == student.Id && qa.QuizId == quiz.Id select qa);

            attempts = quizAttempts.ToList();//.Where(s => s.Where(q => q.StudentId == student.Id));
            
        }
    }
}