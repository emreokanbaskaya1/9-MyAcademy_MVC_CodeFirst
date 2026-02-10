using System;
using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Testimonial
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Client name is required")]
        [MaxLength(100)]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [MaxLength(100)]
        public string Position { get; set; }

        [Required(ErrorMessage = "Comment is required")]
        [MaxLength(1000)]
        public string Comment { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; } = 5;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
