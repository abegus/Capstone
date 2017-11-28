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

namespace Capstone.Controllers
{
    public class QuestionsController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.CoreStandard);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
            return View();
        }


        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(QuestionViewModel question, HttpPostedFileBase file)
         {
             if (ModelState.IsValid)
             {
                //used for converting image file into bytes that can be stored in database
                MemoryStream target = new MemoryStream();
                //question.Picture.InputStream.CopyTo(target);
                file.InputStream.CopyTo(target);
                byte[] data = target.ToArray();

                Question q = new Question()
                {
                    Id = Guid.NewGuid().ToString(),
                    Answer = question.Answer,
                    Type = question.Type,
                    Text = question.Text,
                    Description = question.Description,
                    StandardId = question.StandardId,
                    Picture = data

                };
                 db.Questions.Add(q);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
             return View(question);
         }

         // POST: Questions/Create
         // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
         // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create([Bind(Include = "Id,Type,Picture,Text,Answer,Description,StandardId")] Question question)
         {
             if (ModelState.IsValid)
             {
                 db.Questions.Add(question);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
             return View(question);
         }*/

        // GET: Questions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Type,Picture,Text,Answer,Description,StandardId")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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
