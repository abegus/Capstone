using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Capstone.Models;
using System.Collections;
using System.Web.Mvc;

namespace Capstone.ViewModels
{
   /* public class ClassViewModels
    {
    }*/
    public class CreateClassViewModel{
        [Required]
        [StringLength(50)]
        [Display(Name = "Class Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        /*[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string Id { get; set; }*/
    }

    public class DefaultClassViewModel
    {
        public string classId { get; set; }
        public IEnumerable<Class> Classes { get; set; }
        public SelectList select { get; set; }
    }

    public class ManageClassViewModel
    {
        public Class currentClass { get; set; }

        public List<Quiz> quizzes { get; set; }

        public ICollection<Student> students { get; set; }

        public BrowseViewModel browseModel { get; set; }
    }

    public class ClassQuizOverview
    {
        public Class currentClass { get; set; }

        public Dictionary<Student,int[]> studentAttempts { get; set; }
        public Quiz currentQuiz { get; set; }
    }
}
