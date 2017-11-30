﻿using System;
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
    public class QuestionsController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Questions
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var questions = db.Questions.Include(q => q.CoreStandard);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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
        public ActionResult Create(String QuizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            
            QuestionViewModel ques = new QuestionViewModel();
            ques.QuizId = QuizId;

            ViewBag.FileError = "";
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
            return View(ques);
        }


        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(QuestionViewModel question, HttpPostedFileBase file)
         {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
             {
                //used for converting image file into bytes that can be stored in database
                MemoryStream target = new MemoryStream();
                file.InputStream.CopyTo(target);
                byte[] data = ReduceSize(file.InputStream, 300, 300);
                //if the file is not valid (IE not an image)
                if (data == null)
                {
                    ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
                    ViewBag.FileError = "Not a valid Image file format";
                    return View(question);
                }

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
                Quiz ques = (from x in db.Quizs where x.Id == question.QuizId select x).FirstOrDefault();
                q.Quizs.Add(ques);
                db.Questions.Add(q);
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }

             ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
             return View(question);
         }

        private byte[] ReduceSize(Stream stream, int maxWidth, int maxHeight)
        {
            try
            {
                //if(stream.)
                Image source = Image.FromStream(stream);
                double widthRatio = ((double)maxWidth) / source.Width;
                double heightRatio = ((double)maxHeight) / source.Height;
                double ratio = (widthRatio < heightRatio) ? widthRatio : heightRatio;
                Image thumbnail = source.GetThumbnailImage((int)(source.Width * ratio), (int)(source.Height * ratio), null, IntPtr.Zero);
                using (var memory = new MemoryStream())
                {
                    thumbnail.Save(memory, source.RawFormat);
                    return memory.ToArray();
                }
            }
            catch(System.ArgumentException ex)
            {
                return null;
            }
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

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
