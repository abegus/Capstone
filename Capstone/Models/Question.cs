namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            Answers = new HashSet<Answer>();
            Quizs = new HashSet<Quiz>();
        }

        public string Id { get; set; }

        public int Type { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        public string Text { get; set; }

        [Required]
        [StringLength(50)]
        public string Answer { get; set; }

        public string Description { get; set; }

        [Required]
        [StringLength(128)]
        public string StandardId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual CoreStandard CoreStandard { get; set; }

        //[InverseProperty("Question")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quiz> Quizs { get; set; }
    }
}
