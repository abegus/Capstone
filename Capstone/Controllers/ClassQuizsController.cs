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
using Microsoft.AspNet.Identity;

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
        //public ActionResult Create(BrowseViewModel bm)
        public ActionResult Create(string classId, string quizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.Identity.GetUserId();
            //THIS HANDLES 2 CASES: COMING FROM CLASS,     COMING FROM QUIZ

            BrowseViewModel bm = new BrowseViewModel();

            if(classId != null)
            {
                bm = CreateFromClass(classId,bm);
               //get every quiz that isnt already in your class (that belongs to you FOR NOW. if I am going to change this, then make a browsing search that shows all of them).
                List<Quiz> allUsersQuizzes = (from q in db.Quizs where q.UserId == userId select q).ToList();
                if(bm.quizzes != null)
                {
                    List<Quiz> nonQuizzes = (allUsersQuizzes.Where(q => !bm.quizzes.Any(q2 => q2.Id == q.Id))).ToList();
                    allUsersQuizzes = nonQuizzes;
                }
                List<Class> singleClass = new List<Class>();
                singleClass.Add(bm.currentClass);
                ViewBag.ClassId = new SelectList(singleClass, "Id", "Name");
                ViewBag.QuizId = new SelectList(allUsersQuizzes, "Id", "Name");

                //if there are no quizzes, then redirect to quiz creation
                //CHANGE THIS IN THE FUTURE, IT IS A TEMPORARY FIX
                if(allUsersQuizzes.Count < 1)
                {
                    return RedirectToAction("Create", "Quizs");
                }
                return View();
            }

            else
            {
                bm = CreateFromQuiz(quizId,bm);
            }
            


            //ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name");
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name");
            return View();
        }

        private BrowseViewModel CreateFromQuiz(string quizId, BrowseViewModel bm)
        {
            return bm;
        }

        public BrowseViewModel CreateFromClass(string classId, BrowseViewModel bm)
        {
            Class @class = db.Classes.Find(classId);
            List<Quiz> quizzes = new List<Quiz>();

            foreach (var cq in @class.ClassQuizs)
            {
                quizzes.Add((from q in db.Quizs where q.Id == cq.QuizId select q).FirstOrDefault());
            }

            bm.currentClass = @class;
            bm.quizzes = quizzes;
            return bm;
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

                return RedirectToAction("Advanced", "Classes", new { id = classQuiz.ClassId });
                // RedirectToAction("Index");
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
        public ActionResult Delete(string quizId, string classId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (quizId == null || classId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassQuiz classQuiz = db.ClassQuizs.Find(quizId,classId);
            if (classQuiz == null)
            {
                return HttpNotFound();
            }
            return View(classQuiz);
        }

        // POST: ClassQuizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string quizId, string classId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ClassQuiz classQuiz = db.ClassQuizs.Find(quizId,classId);
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
