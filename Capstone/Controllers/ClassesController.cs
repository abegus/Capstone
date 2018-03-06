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
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

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
            var x = from cl in db.Classes where (from teach in db.Teaches where teach.UserId == userid && teach.ClassId == cl.Id select cl.Id).Any() == true select cl;
            return View(x.ToList());
        }

        // GET: Classes/Analysis/
        public ActionResult Analysis()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var user = db.AspNetUsers.Find(User.Identity.GetUserId());

            Class c = db.Classes.Find(user.DefaultClassId);

            c.Students = c.Students.OrderBy(s => s.Last).ToList();
            return View(c);
        }

        /* public ActionResult ExportToExcel()
         {
             var gv = new GridView();
             var employeeList = (from e in db.Employees
                                 join d in db.Departments on e.DepartmentId equals d.DepartmentId
                                 select new EmployeeViewModel
                                 {
                                     Name = e.Name,
                                     Email = e.Email,
                                     Age = (int)e.Age,
                                     Address = e.Address,
                                     Department = d.DepartmentName
                                 }).ToList();
             gv.DataSource = this.GetEmployeeList();
             gv.DataBind();
             Response.ClearContent();
             Response.Buffer = true;
             Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
             Response.ContentType = "application/ms-excel";
             Response.Charset = "";
             StringWriter objStringWriter = new StringWriter();
             HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
             gv.RenderControl(objHtmlTextWriter);
             Response.Output.Write(objStringWriter.ToString());
             Response.Flush();
             Response.End();
             return View("Index");
         }*/

        //public void ExportClientsListToExcel()
        public ActionResult ExportClientsListToExcel()
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            /* if (xlApp == null)
             {
                 //MessageBox.Show("Excel is not properly installed!!");
                 return;
             }*/

            var user = db.AspNetUsers.Find(User.Identity.GetUserId());
            Class c = db.Classes.Find(user.DefaultClassId);
            c.Students = c.Students.OrderBy(s => s.Last).ToList();

            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Students";
            for (int i = 0; i < c.ClassQuizs.Count; i++)
            {
                xlWorkSheet.Cells[1, i + 2] = c.ClassQuizs.ElementAt(i).Quiz.Name;
            }

            for(int i = 0; i < c.Students.Count; i++)
            {
                Student stud = c.Students.ElementAt(i);
                xlWorkSheet.Cells[i + 2, 1] = stud.First + stud.Last;
                for(int j = 0; j < c.ClassQuizs.Count; j++)
                {
                    ClassQuiz cq = c.ClassQuizs.ElementAt(j);
                    if(cq.QuizAttempts.Where(q => q.StudentId == stud.Id).Count() > 0)
                    {
                        double x = cq.QuizAttempts.Where(q => q.StudentId == stud.Id).FirstOrDefault().numCorrect / (double)cq.Quiz.Questions.Count();
                        xlWorkSheet.Cells[i + 2, j + 2] = x;
                    }
                    else
                    {
                        xlWorkSheet.Cells[i + 2, j + 2] = "NO ATTEMPTS";
                    }
                }
            }
            xlWorkBook.Close(true, misValue, misValue);

            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return RedirectToAction("Analysis", "Classes");

            //Getting the location and file name of the excel to save from user. 
            /* SaveFileDialog saveDialog = new SaveFileDialog();
             saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
             saveDialog.FilterIndex = 2;

                 xlWorkBook.SaveAs(saveDialog.FileName);*/

            // xlWorkBook.SaveAs("C:\\csharp-Excel.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //xlWorkBook.Close(true, misValue, misValue);
            // xlApp.Quit();

            

            //MessageBox.Show("Excel file created , you can find the file d:\\csharp-Excel.xls");

            /* var user = db.AspNetUsers.Find(User.Identity.GetUserId());

             Class c = db.Classes.Find(user.DefaultClassId);

             c.Students = c.Students.OrderBy(s => s.Last).ToList();

             var grid = new System.Web.UI.WebControls.GridView();

             grid.DataSource = //from d in dbContext.diners
                               //where d.user_diners.All(m => m.user_id == userID) && d.active == true 
                               from d in c.Students
                               select new
                               {
                                   FirstName = d.First,
                                   LastName = d.Last,
                                   Email = d.Email

                               };

             grid.DataBind();

             Response.ClearContent();
             Response.AddHeader("content-disposition", "attachment; filename=Exported_Class_Analysis.xls");
             Response.ContentType = "application/excel";
             StringWriter sw = new StringWriter();
             HtmlTextWriter htw = new HtmlTextWriter(sw);

             grid.RenderControl(htw);

             Response.Write(sw.ToString());

             Response.End();*/

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
