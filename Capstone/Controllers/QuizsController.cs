﻿using System;
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
    public class QuizsController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Quizs
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //changing this temporarily because core standards dont exist yet
            //var quizs = db.Quizs;
            var quizs = db.Quizs.Include(q => q.CoreStandard);
            return View(quizs.ToList());
        }

        // GET: Quizs/Advanced/5
        public ActionResult Advanced(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            ViewBag.quizId = id;
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: Quizs/Details/5
        public ActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // GET: Quizs/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
            return View();
        }

        // POST: Quizs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,StandardId,Description")] Quiz quiz)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                Quiz newQuiz = new Quiz
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = quiz.Name,
                    StandardId = quiz.StandardId,
                    Description = quiz.Description
                };

                db.Quizs.Add(newQuiz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", quiz.StandardId);
            return View(quiz);
        }

        // GET: Quizs/Edit/5
        public ActionResult Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", quiz.StandardId);
            return View(quiz);
        }

        // POST: Quizs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,StandardId,Description")] Quiz quiz)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", quiz.StandardId);
            return View(quiz);
        }

        // GET: Quizs/Delete/5
        public ActionResult Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quizs.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST: Quizs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            Quiz quiz = db.Quizs.Find(id);

           /* IQueryable<QuestionQuizs> qqs = (from qq in db.QuestionQuizs where qq.Quiz_Id.Equals(id) select qq);
            foreach(var qq in qqs)
            {
                db.QuestionQuizs.Remove(qq);
            }*/

            //THIS IS A VERY INEFFICIENT QUERY. IT IS LOOKING AT THE ENTIRE QUIZ OBJECT
           // IQueryable<Question> questions = (from q in db.Questions where q.Quizs.Contains(quiz) select q);

            /*foreach(var que in questions)
            {
                quiz.Questions.Remove(que);
            }*/
            
            
            db.Quizs.Remove(quiz);
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
