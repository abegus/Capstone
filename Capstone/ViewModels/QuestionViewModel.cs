using Capstone.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Capstone.ViewModels
{
   /* public class ClassViewModels
    {
    }*/
    public class QuestionViewModel{
        public int Type { get; set; }

        //[DataType(DataType.Upload)]
        //public HttpPostedFileBase Picture { get; set; }

        public int questionIndex { get; set; }

        public string UserId { get; set; }

        public string TypeText { get; set; }

        public string Text { get; set; }

        [Required]
        [StringLength(50)]
        public string Answer { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string StandardId { get; set; }

        public virtual CoreStandard CoreStandard { get; set; }

        //[InverseProperty("Question")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz> Quizs { get; set; }

        public string QuizId { get; set; }
    }
}
