using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Models
{
    [Table("personnel")]
    public class Personnel
    {
        [Required(ErrorMessage = "Personnel ID is required")]
        [Display(Name = "Personnel ID")]
        [Range(1000, 9999, ErrorMessage = "Personnel ID must be between 1000 and 9999")]
        [Key]
        [Column("personnelid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Personnelid { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rank is required")]
        [Column("rank")]
        public string Rank { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unit is required")]
        [Column("unitname")]
        [ForeignKey("UnitnameNavigation")]
        public string Unitname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [Column("contactnumber")]
        public string Contactnumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [Column("email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Column("joiningdate")]
        public DateTime? Joiningdate { get; set; }

        [Column("emergencycontact")]
        public string Emergencycontact { get; set; } = string.Empty;

        [Column("bloodgroup")]
        public string Bloodgroup { get; set; } = string.Empty;

        [Column("weaponassigned")]
        public string Weaponassigned { get; set; } = string.Empty;

        [Column("dutystatus")]
        public string Dutystatus { get; set; } = string.Empty;

        public virtual Unit? UnitnameNavigation { get; set; }
        public virtual ICollection<Mission> Missioncodes { get; set; } = new List<Mission>();
    }
}
