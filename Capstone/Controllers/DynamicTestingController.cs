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

        // GET: DynamicTesting/Take
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

            vm.studentId = studentId;
            vm.classId = classId;
            vm.quizId = quizId;

            vm.jsonQuestions = new List<JsonQuestion>();

            string[] questionIds = new string[vm.questions.Count()];
            int[] answers = new int[vm.questions.Count()];
            int index = 0;
            foreach(var q in vm.questions)
            {
                questionIds[index] = q.Id;
                answers[index] = -1;
                index++;
            }
            vm.questionIds = questionIds;
            vm.answers = answers;

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

        // POST: DynamicTesting/Take
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Take(DynamicViewModel vm)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                int index = 0;
                foreach(var que in vm.questionIds)
                {
                    ClassQuiz cq = db.ClassQuizs.Find(vm.quizId, vm.classId);
                    Answer ans = new Answer()
                    {
                        Id = Guid.NewGuid().ToString(),
                        QuestionId = que,
                        Question = db.Questions.Find(que),
                        StudentId = vm.studentId,
                        QuizId = vm.quizId,
                        Student = db.Students.Find(vm.studentId),
                        ClassId = vm.cla.Id,
                        Correct = vm.answers[index],
                        Date = DateTime.UtcNow.Date,
                        ClassQuiz = cq
                    };

                    db.Answers.Add(ans);
                    index++;
                }
                db.SaveChanges();

                return RedirectToAction("Advanced", "Classes", new { id = vm.cla.Id });
            }

            return RedirectToAction("Advanced", "Classes", new { id = vm.cla.Id });
        }

        public PartialViewResult GetQuestion(string id)
        {
            Question q = db.Questions.Find(id);

            return PartialView(q);
        }
    }
}