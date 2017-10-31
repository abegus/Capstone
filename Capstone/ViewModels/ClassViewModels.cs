using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capstone.ViewModels
{
   /* public class ClassViewModels
    {
    }*/
    public class CreateClassViewModel{
        [Required]
        [Display(Name = "Class Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        /*[DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string Id { get; set; }*/
    }
}
