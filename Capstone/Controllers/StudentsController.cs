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
    public class StudentsController : Controller
    {
        private MasterModel db = new MasterModel();
        

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        /* partial Modal view returned in Home Index */
        public PartialViewResult Overview(string id)
        {
            Student s = db.Students.Find(id);
            Dictionary<ClassQuiz, QuizAttempt> studentAttempts = new Dictionary<ClassQuiz, QuizAttempt>();
            foreach (var cq in s.Class.ClassQuizs)
            {
                //grab the most recent quizAttempt if one exists in the last X days?
                var attempt = (from qa in db.QuizAttempts
                                       where qa.StudentId == id && cq.QuizId == qa.QuizId
                                       group qa by qa.QuizId into grop
                                       select grop.OrderByDescending(g => g.date));

                if(attempt == null || !attempt.Any())
                {
                    studentAttempts[cq] = null;
                }
                else
                {
                    if (!attempt.Any())
                        studentAttempts[cq] = null;
                    else
                        studentAttempts[cq] = attempt.First().First();
                }
            }
            ViewBag.StudentName = s.First + " " + s.Last;
            ViewBag.StudentId = id;

            //return PartialView(s);
            return PartialView(studentAttempts);
        }

        // GET: Students/Create
        public PartialViewResult Create(String classId)
        {
            //do this to maintain the student to class relationship for a backtrack
            Student student = new Student();
            student.ClassId = classId;
            
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name");
            return PartialView(student);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "First,Last,Email,ClassId")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid().ToString();
                db.Students.Add(student);
                db.SaveChanges();
                

                return RedirectToAction("Advanced","Classes", new { Id = student.ClassId});
            }

            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Edit/5
        public PartialViewResult Edit(string id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            Student student = db.Students.Find(id);
           /* if (student == null)
            {
                return HttpNotFound();
            }*/
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", student.ClassId);
            return PartialView(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,First,Last,Email,ClassId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Advanced", "Classes", new { Id = student.ClassId });
            }
            ViewBag.ClassId = new SelectList(db.Classes, "Id", "Name", student.ClassId);
            return View(student);
        }

        // GET: Students/Delete/5
        public PartialViewResult Delete(string id)
        {
            /*(if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            Student student = db.Students.Find(id);
            /*if (student == null)
            {
                return HttpNotFound();
            }*/
            return PartialView(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            var classId = student.ClassId;
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Advanced", "Classes", new { Id = classId });
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
