using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class Personnel
    {
        public int Personnelid { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Rank { get; set; } = string.Empty;
        
        [Required]
        public string Unitname { get; set; } = string.Empty;
        
        [Required]
        public string Contactnumber { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public DateTime? Joiningdate { get; set; }
        
        [Required]
        public string Emergencycontact { get; set; } = string.Empty;
        
        [Required]
        public string Bloodgroup { get; set; } = string.Empty;
        
        [Required]
        public string Weaponassigned { get; set; } = string.Empty;
        
        [Required]
        public string Dutystatus { get; set; } = string.Empty;
        
        public virtual Unit? UnitnameNavigation { get; set; }
        public virtual ICollection<Mission> Missioncodes { get; set; } = new List<Mission>();
    }
}
