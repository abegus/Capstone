namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        public string Id { get; set; }

        public int Correct { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(128)]
        public string QuestionId { get; set; }

        [Required]
        [StringLength(128)]
        public string QuizId { get; set; }

        [Required]
        [StringLength(128)]
        public string ClassId { get; set; }

        [Required]
        [StringLength(128)]
        public string StudentId { get; set; }

        public virtual ClassQuiz ClassQuiz { get; set; }

        public virtual Question Question { get; set; }

        public virtual Student Student { get; set; }
    }
}
