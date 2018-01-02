using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Capstone.Helpers
{
    public class JsonQuestion
    {
        public string Id { get; set; }

        public int Type { get; set; }

        [Column(TypeName = "image")]
        public byte[] Picture { get; set; }

        public string Text { get; set; }
        
        public string Answer { get; set; }

        public string Description { get; set; }
        
        public string StandardId { get; set; }
        
    }
}