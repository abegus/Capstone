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
    public class CoreStandardsController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: CoreStandards
        public ActionResult Index()
        {
            return View(db.CoreStandards.ToList());
        }

        // GET: CoreStandards/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreStandard coreStandard = db.CoreStandards.Find(id);
            if (coreStandard == null)
            {
                return HttpNotFound();
            }
            return View(coreStandard);
        }

        // GET: CoreStandards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoreStandards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Grade,Description")] CoreStandard coreStandard)
        {
            if (ModelState.IsValid)
            {
                db.CoreStandards.Add(coreStandard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coreStandard);
        }

        // GET: CoreStandards/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreStandard coreStandard = db.CoreStandards.Find(id);
            if (coreStandard == null)
            {
                return HttpNotFound();
            }
            return View(coreStandard);
        }

        // POST: CoreStandards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Grade,Description")] CoreStandard coreStandard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coreStandard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coreStandard);
        }

        // GET: CoreStandards/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreStandard coreStandard = db.CoreStandards.Find(id);
            if (coreStandard == null)
            {
                return HttpNotFound();
            }
            return View(coreStandard);
        }

        // POST: CoreStandards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CoreStandard coreStandard = db.CoreStandards.Find(id);
            db.CoreStandards.Remove(coreStandard);
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
