using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Models;

namespace Capstone.ViewModels
{
    public class DynamicViewModel
    {
        public Student student;
        public Class @class;
        public Quiz quiz;
        public ClassQuiz cq;

        //all of the questions
        public List<Question> questions;

    }
}