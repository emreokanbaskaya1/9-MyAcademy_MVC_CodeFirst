using System;
using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string FacebookUrl { get; set; }

        [MaxLength(200)]
        public string TwitterUrl { get; set; }

        [MaxLength(200)]
        public string InstagramUrl { get; set; }

        [MaxLength(200)]
        public string LinkedInUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
