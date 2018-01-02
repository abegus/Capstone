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

        //for storing in view...
        public string studentId { get; set; }
        public string classId { get; set; }
        public string quizId { get; set; }

        //for who knows what...?
        public Student student;
        public Class cla { get; set; }
        public Quiz quiz {get;set;}
        public ClassQuiz cq { get; set; }

        //id's of all questions in an index;
        public string[] questionIds { get; set; }
        // answers for all of the questions
        public int[] answers { get; set; } // -1 for incomplete, 0 for incorrect answer, 1 for correct answer

        //all of the questions
        public List<Question> questions { get; set; }
        //all of the questions to use in the view
        public List<JsonQuestion> jsonQuestions { get; set; }

    }
}