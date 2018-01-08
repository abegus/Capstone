namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuizAttempt")]
    public partial class QuizAttempt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QuizAttempt()
        {
            Answers = new HashSet<Answer>();
        }

        [Required]
        [StringLength(128)]
        public string Id { get; set; }
        
        [Required]
        public DateTime date { get; set; }

        public int numCorrect { get; set; }

        public int toalQuestions { get; set; }



        [Required]
        [StringLength(128)]
        public string QuizId { get; set; }

        [Required]
        [StringLength(128)]
        public string ClassId { get; set; }

        [Required]
        [StringLength(128)]
        public string StudentId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ClassQuiz ClassQuiz { get; set; }

        public virtual Student Student { get; set; }
    }
}