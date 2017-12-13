namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quiz")]
    public partial class Quiz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quiz()
        {
            ClassQuizs = new HashSet<ClassQuiz>();

            //added this for EF issue not mapping many to many relationship
            Questions = new HashSet<Question>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string StandardId { get; set; }

        public string Description { get; set; }



        //  ADDING OWNERSHIP TO QUIZZES AND QUESITONS
        public string UserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClassQuiz> ClassQuizs { get; set; }

        public virtual CoreStandard CoreStandard { get; set; }

        //public virtual Question Question { get; set; }
        //[InverseProperty("Quiz")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }
    }
}
