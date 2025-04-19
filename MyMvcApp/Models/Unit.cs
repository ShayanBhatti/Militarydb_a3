using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Unit
    {
        [Required]
        public string Unitname { get; set; } = string.Empty;
        
        [Required]
        public string Unitlocation { get; set; } = string.Empty;
        
        public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();
    }
}
