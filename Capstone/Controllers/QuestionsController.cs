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
using Microsoft.AspNet.Identity;

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

            var questions = db.Questions;//.Include(q => q.CoreStandard);
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
        public PartialViewResult Create(string QuizId, int NextIndex)
        {
            QuestionViewModel ques = new QuestionViewModel();
            ques.QuizId = QuizId;
            ques.UserId = User.Identity.GetUserId();

            ques.questionIndex = NextIndex;

            ViewBag.FileError = "";
            ViewBag.quizId = QuizId;
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
            return PartialView(ques);
        }

        // GET: Questions/Create
        /*public ActionResult Create(String QuizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            
            QuestionViewModel ques = new QuestionViewModel();
            ques.QuizId = QuizId;
            ques.UserId = User.Identity.GetUserId();

            ViewBag.FileError = "";
            ViewBag.quizId = QuizId;
            ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
            return View(ques);
        }*/


        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(QuestionViewModel question, HttpPostedFileBase file, string quizId)
         {
            if (!User.Identity.IsAuthenticated) 
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid && question.TypeText != null)
            {
                int type = 0;
                if (question.TypeText.Equals("Text"))
                {
                    type = 1;
                }

                Question q = new Question()
                {
                    Id = Guid.NewGuid().ToString(),
                    Answer = question.Answer,
                    Type = type,
                    Text = question.Text,
                    Description = question.Description,
                    //StandardId = question.StandardId,
                    UserId = User.Identity.GetUserId(),
                    QuestionIndex = question.questionIndex
                   // Picture = data

                };

                //if its a picture
                if (type == 0)
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

                    q.Picture = data;
                }

               
                Quiz ques = (from x in db.Quizs where x.Id == question.QuizId select x).FirstOrDefault();
                q.Quizs.Add(ques);
                db.Questions.Add(q);
                 db.SaveChanges();

                if (quizId != null)
                    return RedirectToAction("Advanced", "Quizs", new { id = quizId });
                return RedirectToAction("Index", "Questions");
            }

            // ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
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
        public PartialViewResult Edit(string id, string quizId)
        {
            /*if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            Question question = db.Questions.Find(id);
            /*if (question == null)
            {
                return HttpNotFound();
            }*/
            ViewBag.quizId = quizId;
            //ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
            return PartialView(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Type,Picture,Text,Answer,Description")] Question question, HttpPostedFileBase file, string quizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //if its a picture
                if (question.Type == 0)
                {
                    if(file == null)
                    {
                        ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name");
                        ViewBag.FileError = "Not a valid Image file format";
                        return View(question);
                    }
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

                    question.Picture = data;
                }

                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");

                if (quizId != null)
                    return RedirectToAction("Advanced", "Quizs", new { id = quizId });
                return RedirectToAction("Index", "Questions");
            }
            //ViewBag.StandardId = new SelectList(db.CoreStandards, "Id", "Name", question.StandardId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public PartialViewResult Delete(string id, string quizId)
        {
            /*if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            ViewBag.quizId = quizId;
            Question question = db.Questions.Find(id);
            /*if (question == null)
            {
                return HttpNotFound();
            }*/
            return PartialView(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string quizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            // I WILL HAVE TO CHANGE THE DELETION FOR THE RELATIONSHIPS IN QUIZCONTROLLER DELETE AS WELL
            
            Question question = db.Questions.Find(id);
            Quiz q = db.Quizs.Find(quizId);

            //Problem is that I am not passing the quizId (for the respective quiz) that has the question.
            //I also need to pass the quiz data to the Delete (from the quiz views) ___?
            //https://stackoverflow.com/questions/20942926/pass-multiple-parameters-in-html-beginform-mvc4-controller-action
            //QuestionQuiz qq = (from x in db.QuestionQuizs where x.Question_Id.Equals(id) && x.Quiz_Id.Equals(quizId) select x).FirstOrDefault();


            //if this, then it is not deleted from a quiz (quizID is null)
            if(q != null)
            {
                q.Questions.Remove(question);
            }
            
            //this would delete it if it is the only entiyt left. Still have to implement
            db.Questions.Remove(question);

            db.SaveChanges();
            if(quizId != null)
                return RedirectToAction("Advanced","Quizs",new { id = quizId });
            return RedirectToAction("Index", "Questions");
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
