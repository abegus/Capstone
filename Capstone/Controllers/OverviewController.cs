using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Capstone.ViewModels;
using System.IO;
using System.Drawing;

namespace Capstone.Controllers
{
    public class OverviewController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Overview
        /* Input: This takes a classId and quizId as input
         * Called By: Called from Classes => Advanced on a given classQuiz
         * Output: Output is a ClassQuizOverview object.
         */
        public ActionResult Index(string classId, string quizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if(classId == null || quizId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ClassQuizOverview viewModel = constructModel(classId, quizId);

            return View(viewModel);
        }

        public ClassQuizOverview constructModel(string classId, string quizId)
        {
            ClassQuizOverview vm = new ClassQuizOverview();
            vm.studentAttempts = new Dictionary<Student, int[]>(); //an array storing the number of attempts / the total size of the quiz
            vm.currentClass = db.Classes.Find(classId);
            vm.currentQuiz = db.Quizs.Find(quizId);
            var classQuiz = db.ClassQuizs.Find(quizId, classId);
            var students = vm.currentClass.Students;

            int quizSize = vm.currentQuiz.Questions.Count();
            //grab a count of all attempts for a given Student in the class for that given class quiz. This will be mapped to the dictionary
            //for the given student.
            foreach (var stud in students)
            {
                //pre change
                /*var numAttempts = (from answer in db.Answers
                                   where answer.ClassQuiz.QuizId == quizId && answer.ClassQuiz.ClassId == classId && answer.StudentId == stud.Id
                                   select answer).Count();*/
                var numAttempts = (from qa in db.QuizAttempts
                                   where qa.QuizId == quizId && qa.ClassId == classId && qa.StudentId == stud.Id
                                   select qa).Count();

                int[] tuple = { numAttempts, quizSize };
                vm.studentAttempts.Add(stud, tuple);
            }


            return vm;
        }
    }
    
}