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

namespace Capstone.Controllers
{
    public class ClassQuizsController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: ClassQuizs
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var classQuizs = db.ClassQuizs.Include(c => c.Class).Include(c => c.Quiz);
            return View(classQuizs.ToList());
        }

        // GET: ClassQuizs/Details/5
        public ActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassQuiz classQuiz = db.ClassQuizs.Find(id);
            if (classQuiz == null)
            {
                return HttpNotFound();
            }
            return View(classQuiz);
        }

        // GET: ClassQuizs/Create
        public ActionResult Create(ManageClassViewModel vm)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //get every quiz that isnt already in your class (that belongs to you FOR NOW. if I am going to change this, then make a browsing search that shows all of them).

            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name");
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name");
            return View();
        }

        // POST: ClassQuizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuizId,ClassId,Description")] ClassQuiz classQuiz)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.ClassQuizs.Add(classQuiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", classQuiz.ClassId);
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name", classQuiz.QuizId);
            return View(classQuiz);
        }

        // GET: ClassQuizs/Edit/5
        public ActionResult Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassQuiz classQuiz = db.ClassQuizs.Find(id);
            if (classQuiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", classQuiz.ClassId);
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name", classQuiz.QuizId);
            return View(classQuiz);
        }

        // POST: ClassQuizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuizId,ClassId,Description")] ClassQuiz classQuiz)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(classQuiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", classQuiz.ClassId);
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name", classQuiz.QuizId);
            return View(classQuiz);
        }

        // GET: ClassQuizs/Delete/5
        public ActionResult Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassQuiz classQuiz = db.ClassQuizs.Find(id);
            if (classQuiz == null)
            {
                return HttpNotFound();
            }
            return View(classQuiz);
        }

        // POST: ClassQuizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ClassQuiz classQuiz = db.ClassQuizs.Find(id);
            db.ClassQuizs.Remove(classQuiz);
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
