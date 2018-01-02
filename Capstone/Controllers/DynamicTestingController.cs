using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Capstone.ViewModels;
using Capstone.Helpers;

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
            vm.cla = db.Classes.Find(classId);
            vm.quiz = db.Quizs.Find(quizId);
            vm.cq = db.ClassQuizs.Find(classId, quizId);
            vm.questions = vm.quiz.Questions.ToList();

            vm.jsonQuestions = new List<JsonQuestion>();

            string[] questionIds = new string[vm.questions.Count()];
            int index = 0;
            foreach(var q in vm.questions)
            {
                questionIds[index] = q.Id;
                index++;
            }
            vm.questionIds = questionIds;

            foreach(var question in vm.questions)
            {
                JsonQuestion q = new JsonQuestion
                {
                    Id = question.Id,
                    Text = question.Text,
                    Type = question.Type,
                    Picture = question.Picture,
                    Description = question.Description,
                    StandardId = question.StandardId,
                    Answer = "-1"
                };
                vm.jsonQuestions.Add(q);
            }
            //all of the previous answers?ue
           /* IQueryable < Answer > answers = (from ans in db.Answers
                                             where ans.ClassId == classId &&
                                             ans.ClassQuiz.QuizId == quizId &&
                                             ans.StudentId == studentId
                                             select ans);*/
            return View(vm);
        }

        public PartialViewResult GetQuestion(string id)
        {
            Question q = db.Questions.Find(id);

            return PartialView(q);
        }
    }
}