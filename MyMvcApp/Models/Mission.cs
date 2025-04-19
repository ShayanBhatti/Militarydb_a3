using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    [Table("mission")]
    public class Mission
    {
        [Key]
        [Column("missioncode")]
        [Required]
        [StringLength(10)]
        public string Missioncode { get; set; } = string.Empty;

        [Column("missiondate")]
        public DateTime? Missiondate { get; set; }

        public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();

        // Convert local time to UTC before saving
        public void ConvertToUtc()
        {
            if (Missiondate.HasValue)
            {
                Missiondate = DateTime.SpecifyKind(Missiondate.Value, DateTimeKind.Utc);
            }
        }
    }
}
