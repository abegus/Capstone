namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClassQuiz")]
    public partial class ClassQuiz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ClassQuiz()
        {
            QuizAttempts = new HashSet<QuizAttempt>();
        }

        [Key]
        [Column(Order = 0)]
        public string QuizId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string ClassId { get; set; }

        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }

        public virtual Class Class { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
