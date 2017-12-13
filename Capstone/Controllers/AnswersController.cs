using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class AnswersController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Answers
        public ActionResult Index()
        {
            var answers = db.Answers.Include(a => a.ClassQuiz).Include(a => a.Question).Include(a => a.Student);
            return View(answers.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: Answers/Create
        public ActionResult Create()
        {
            ViewBag.QuizId = new SelectList(db.ClassQuizs, "QuizId", "Description");
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text");
            ViewBag.StudentId = new SelectList(db.Students, "Id", "First");
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Correct,Date,QuestionId,QuizId,ClassId,StudentId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Answers.Add(answer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuizId = new SelectList(db.ClassQuizs, "QuizId", "Description", answer.QuizId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", answer.QuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "First", answer.StudentId);
            return View(answer);
        }

        // GET: Answers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuizId = new SelectList(db.ClassQuizs, "QuizId", "Description", answer.QuizId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", answer.QuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "First", answer.StudentId);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Correct,Date,QuestionId,QuizId,ClassId,StudentId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuizId = new SelectList(db.ClassQuizs, "QuizId", "Description", answer.QuizId);
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", answer.QuestionId);
            ViewBag.StudentId = new SelectList(db.Students, "Id", "First", answer.StudentId);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
