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
using System.Collections;

namespace Capstone.Controllers
{
    public class ClassesController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Classes
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login","Account");

            var userid = User.Identity.GetUserId();
            //db.
            var x = from cl in db.Classes where (from teach in db.Teaches where teach.UserId == userid && teach.ClassId == cl.Id select cl.Id).Any() == false select cl ;
            //any() == false models a NOT EXISTS
            //var b = 2;
           // 
            return View(db.Classes.ToList());
        }

        // GET: Classes/Advanced/5
        public ActionResult Advanced(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageClassViewModel vm = new ManageClassViewModel();
            //BrowseViewModel bm = new BrowseViewModel();
            
            Class @class = db.Classes.Find(id);

            var students = @class.Students;
            List<Quiz> quizzes = new List<Quiz>();
            
            foreach(var cq in @class.ClassQuizs)
            {
                quizzes.Add((from q in db.Quizs where q.Id == cq.QuizId select q).FirstOrDefault());
            }

            vm.currentClass = @class;
            vm.quizzes = quizzes;
            vm.students = students;

           // bm.currentClass = @class;
            //bm.quizzes = quizzes;

            //vm.browseModel = bm;
            


            if (@class == null)
            {
                return HttpNotFound();
            }
            //return View(@class);

            return View(vm);
        }

        //GET: Classes/AddQuiz
       /* public ActionResult AddQuiz(ManageClassViewModel vm)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (vm == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(vm);
        }*/

        //POST for Advanced, need this if dynamic creation

        // GET: Classes/Details/5
        public ActionResult Details(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // GET: Classes/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateClassViewModel @class)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                //Maybe create another "Data helper" class that holds a method, "create from vm", update from vm, etc,
                //lots of decision making for different input data for the method, but consistent updating. THis would remove
                // the data aspect of the controllers into another module
                Class newClass = new Class
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = @class.Name,
                    SchoolName = @class.SchoolName
                };
                db.Classes.Add(newClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Guid.NewGuid().ToString();

            return View(@class);
        }

        // GET: Classes/Edit/5
        public ActionResult Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,SchoolName")] Class @class)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(@class).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@class);
        }

        // GET: Classes/Delete/5
        public ActionResult Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return HttpNotFound();
            }
            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            Class @class = db.Classes.Find(id);
            db.Classes.Remove(@class);
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

        private ActionResult PreConditionCheck()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return null;
        }
    }
}
