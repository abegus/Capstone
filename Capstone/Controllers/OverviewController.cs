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

            ClassQuizOverview viewModel = new ClassQuizOverview();
            viewModel.studentAttempts = new Dictionary<Student, int>();
            viewModel.currentClass = db.Classes.Find(classId);
            viewModel.currentQuiz = db.Quizs.Find(quizId);
            var classQuiz = db.ClassQuizs.Find(quizId, classId);
            var students = viewModel.currentClass.Students;

            //grab a count of all attempts for a given Student in the class for that given class quiz. This will be mapped to the dictionary
            //for the given student.
            foreach(var stud in students)
            {
                var numAttempts = (from answer in db.Answers
                                   where answer.ClassQuiz.QuizId == quizId && answer.ClassQuiz.ClassId == classId && answer.StudentId == stud.Id
                                   select answer).Count();
                viewModel.studentAttempts.Add(stud, numAttempts);
            }

            return View(viewModel);
        }
    }
}