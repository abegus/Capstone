namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            QuizAttempts = new HashSet<QuizAttempt>();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string First { get; set; }

        [Required]
        [StringLength(50)]
        public string Last { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(128)]
        public string ClassId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }

        public virtual Class Class { get; set; }
    }
}
