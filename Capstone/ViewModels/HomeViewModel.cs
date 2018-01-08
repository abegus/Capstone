using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Models;

namespace Capstone.ViewModels
{
    public class HomeViewModel
    {
        public Class clas { get; set; }
        public Quiz[] quizzes { get; set; }
    }
}