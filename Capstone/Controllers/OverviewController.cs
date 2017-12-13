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

namespace Capstone.Controllers
{
    public class OverviewController : Controller
    {
        private MasterModel db = new MasterModel();

        // GET: Overview
        public ActionResult Index(string classId, string QuizId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

           /* var classes = from 

            var quizzes = (from q in db.Quizs where q.);*/
            return View();
        }
    }
}