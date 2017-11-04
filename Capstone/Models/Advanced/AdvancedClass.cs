using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Models.Advanced
{
    public class AdvancedClass
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string SchoolName { get; set; }

        public HashSet<Student> Students { get; set; }

        public HashSet<ClassQuiz> ClassQuizs { get; set; }

        // holds ClassId and UserId, Role of teachers associated with this class.
        public HashSet<Teach> Teachers { get; set; }
    }

   /* public class ClassQuizzes 
    {
        public HashSet<ClassQuiz> Quizzes { get; set; }
    }

    public class ClassInfo
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string SchoolName { get; set; }
    }*/
}