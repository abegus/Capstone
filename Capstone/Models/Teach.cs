namespace Capstone.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Teach
    {
        [Key]
        [Column(Order = 0)]
        public string ClassId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }

        public int Role { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual Class Class { get; set; }
    }
}
