using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Capstone.Models;
using System.Collections;
using System.Linq;

namespace Capstone.ViewModels
{
    /*public class BrowseQuizViewModels
    {
    }*/
    public class BrowseViewModel
    {
        public IQueryable<Class> classes { get; set; }

        public Class currentClass { get; set; }

        public Quiz currentQuiz { get; set; }

        public List<Quiz> quizzes { get; set; }

        public string UserId { get; set; }
    }
}