using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Capstone.Models;
using System.Collections;

namespace Capstone.ViewModels
{
    /*public class BrowseQuizViewModels
    {
    }*/
    public class BrowseYourQuizs
    {
        public Class currentClass { get; set; }

        public List<Quiz> quizzes { get; set; }
    }
}