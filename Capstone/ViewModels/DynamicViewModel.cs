using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Capstone.Models;
using Capstone.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Capstone.ViewModels
{
    public class DynamicViewModel
    {
        [Key]
        public String key = "1";

        public Student student;
        public Class cla;
        public Quiz quiz;
        public ClassQuiz cq;

        //id's of all questions in an index;
        public string[] questionIds;
        //all of the questions
        public List<Question> questions;
        //all of the questions to use in the view
        public List<JsonQuestion> jsonQuestions;

    }
}