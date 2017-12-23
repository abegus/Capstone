using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Capstone.ViewModels;

namespace Capstone.Controllers
{
    public class DynamicTestingController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: DynamicTesting
        public ActionResult Take(string studentId, string classId, string quizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            DynamicViewModel vm = new DynamicViewModel();
            vm.student = db.Students.Find(studentId);
            vm.@class = db.Classes.Find(classId);
            vm.quiz = db.Quizs.Find(quizId);
            vm.cq = db.ClassQuizs.Find(classId, quizId);
            vm.questions = vm.quiz.Questions.ToList();
            //all of the previous answers?ue
           /* IQueryable < Answer > answers = (from ans in db.Answers
                                             where ans.ClassId == classId &&
                                             ans.ClassQuiz.QuizId == quizId &&
                                             ans.StudentId == studentId
                                             select ans);*/
            return View(vm);
        }
    }
}