using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Slider
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subtitle is required")]
        [MaxLength(200)]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [MaxLength(200)]
        public string ButtonText { get; set; }

        [MaxLength(200)]
        public string ButtonUrl { get; set; }

        public int OrderNumber { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
