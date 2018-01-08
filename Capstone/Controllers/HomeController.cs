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
                return RedirectToAction("Classes", "Index");
            }

            HomeViewModel vm = new HomeViewModel();
            //get the first class and load all the data (TEMPORARY AS THE DEFAULT) because I haven't implemented a default class yet...
            vm.clas = db.Classes.Find(classIds[0]);
            vm.quizzes = new Quiz[vm.clas.ClassQuizs.Count];
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