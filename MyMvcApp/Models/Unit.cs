using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    [Table("unit")]
    public class Unit
    {
        [Key]
        [Required]
        [Column("unitname")]
        public string Unitname { get; set; } = string.Empty;
        
        [Required]
        [Column("unitlocation")]
        public string Unitlocation { get; set; } = string.Empty;

        /*
        [Required]
        [Column("commander")]
        public string Commander { get; set; } = string.Empty;

        [Required]
        [Column("type")]
        public string Type { get; set; } = string.Empty;
        */

        public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();
    }
}
