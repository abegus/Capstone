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
                return RedirectToAction("Login", "Account");

            var userid = User.Identity.GetUserId();
            //db.
            var x = from cl in db.Classes where (from teach in db.Teaches where teach.UserId == userid && teach.ClassId == cl.Id select cl.Id).Any() == true select cl;
            //any() == false models a NOT EXISTS
            //var b = 2;
            // 
            //return View(db.Classes.ToList());
            return View(x.ToList());
        }

        // GET: Classes/Analysis/
        public ActionResult Analysis()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var user = db.AspNetUsers.Find(User.Identity.GetUserId());

            Class c = db.Classes.Find(user.DefaultClassId);
            //c.Students = c.Students.OrderBy(s => s.Last);
            return View(c);
        }

        // GET: Classes/Advanced/5
        public ActionResult Advanced(string id, string sortOrder)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageClassViewModel vm = new ManageClassViewModel();
            //BrowseViewModel bm = new BrowseViewModel();

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AttemptSortParm = sortOrder == "attempts_asc" ? "attempts_desc" : "attempts_asc";

            Class @class = db.Classes.Find(id);
            vm.currentClass = @class;
            vm.students = @class.Students;

            if (@class == null)
            {
                return HttpNotFound();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vm.students = vm.students.OrderByDescending(s => s.Last);
                    break;
                case "attempts_asc":
                    vm.students = vm.students.OrderBy(s => s.QuizAttempts);
                    break;
                case "attempts_desc":
                    vm.students = vm.students.OrderByDescending(s => s.QuizAttempts);
                    break;
                default:
                    //name_asc
                    vm.students = vm.students.OrderBy(s => s.Last);
                    break;
            }

            return View(vm);
        }


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
                Teach t = new Teach
                {
                    UserId = User.Identity.GetUserId(),
                    ClassId = newClass.Id
                };

                //if the User doesn't have a default class, set this class as their default:
                var user = db.AspNetUsers.Find(User.Identity.GetUserId());
                if (user.DefaultClassId == null)
                {
                    user.DefaultClassId = newClass.Id;
                }

                db.Classes.Add(newClass);
                db.Teaches.Add(t);
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

        // GET: Classes/ChangeDefault/5
        public ActionResult ChangeDefault()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userid = User.Identity.GetUserId();
            var x = from cl in db.Classes where (from teach in db.Teaches where teach.UserId == userid && teach.ClassId == cl.Id select cl.Id).Any() == true select cl;
            if (x == null)
            {
                return HttpNotFound();
            }
            DefaultClassViewModel vm = new DefaultClassViewModel();
            vm.select = new SelectList(x, "Id", "Name", 1);
            vm.classId = "-1";
            //vm.Classes = x;

            return View(vm);
        }

        // POST: Classes/ChangeDefault/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeDefault(string classId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            user.DefaultClassId = classId;
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
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
