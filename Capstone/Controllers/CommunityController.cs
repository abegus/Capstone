using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Capstone.Controllers
{
    public class CommunityController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Community
        //Index will serve as the browsing of the public quizzes that don't belong to the users class quizzes
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            
            //get the user
            var curUser = db.AspNetUsers.Find(User.Identity.GetUserId());
            //get the default class
            var defaultClass = db.Classes.Find(curUser.DefaultClassId);
            //get all of the quizzes that aren't a part of the class defaultClass
            IEnumerable<Quiz> publicQuizzes = (from quiz in db.Quizs
                                 where !quiz.ClassQuizs.Any(cq => cq.ClassId == defaultClass.Id)//defaultClass.ClassQuizs.Contains(cq))
                                 select quiz).AsEnumerable();

            /* DO SORTING BASED ON: (1) Most popular.   (2) Core Standard.   (3) Keyword Search.    (4) ???   */
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PopularSortParm = sortOrder == "popular_asc" ? "popular_desc" : "popular_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                publicQuizzes = publicQuizzes.Where(s => s.CoreStandard.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    publicQuizzes = publicQuizzes.OrderByDescending(s => s.Name);
                    break;
                case "popular_asc":
                    publicQuizzes = publicQuizzes.OrderByDescending(s => s.ClassQuizs.Count());
                    break;
                case "popular_desc":
                    publicQuizzes = publicQuizzes.OrderBy(s => s.ClassQuizs.Count());
                    break;
                default:
                    publicQuizzes = publicQuizzes.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(publicQuizzes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Community/Details/5
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

        // GET: Community/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name");
            ViewBag.QuizId = new SelectList(db.Quizs, "Id", "Name");
            return View();
        }

        // POST: Community/Create
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

        // GET: Community/Edit/5
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

        // POST: Community/Edit/5
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

        // GET: Community/Delete/5
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

        // POST: Community/Delete/5
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
