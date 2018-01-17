using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Microsoft.AspNet.Identity;
using Capstone.ViewModels;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        private MasterModel db = new MasterModel();

        public ActionResult Index()
        {
            //if the user isn't logged in, redirect them to the about page
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("About");
            }
            var userId = User.Identity.GetUserId();

            //all of the class IDs
            string[] classIds = (from cl in db.Classes where (from teach in db.Teaches where teach.UserId == userId && teach.ClassId == cl.Id select cl.Id).Any() == true select cl.Id).ToArray();

            //if there exists a class that they teach, grab it, otherwise redirect them to create a class
            if(classIds.Length < 1) {
                return RedirectToAction("Index", "Classes");
            }

            HomeViewModel vm = new HomeViewModel();
            AspNetUser u = db.AspNetUsers.Find(userId);
            Class c = db.Classes.Find(u.DefaultClassId);
            c.Students = c.Students.OrderBy(s => s.Last).ToList();

            //get the default class if it exists,
            if (c != null)
            {
                vm.clas = c;
            }
            //OR get the first class returned by the database
            else
            {
                vm.clas = db.Classes.Find(classIds[0]);
            }
            vm.quizzes = new Quiz[vm.clas.ClassQuizs.Count];
            vm.classQuizzes = new ClassQuiz[vm.clas.ClassQuizs.Count];
            int index = 0;
            foreach(var cq in vm.clas.ClassQuizs)
            {
                
                vm.quizzes[index] = cq.Quiz;
                index++;
            }

            return View( vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}