using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Mission
    {
        [Required]
        public string Missioncode { get; set; } = string.Empty;
        
        public DateTime? Missiondate { get; set; }
        
        public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();
    }
}
