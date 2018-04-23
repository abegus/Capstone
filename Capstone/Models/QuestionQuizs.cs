using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Capstone.Models
{
    [Table("QuestionQuizs")]
    public class QuestionQuizs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuestionQuizs()
        {
            // Answers = new HashSet<Answer>();
        }

        [Key]
        [Column(Order = 0)]
        public string Quiz_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Question_Id { get; set; }

        public int QuestionIndex { get; set; }

        public virtual Question Question { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}