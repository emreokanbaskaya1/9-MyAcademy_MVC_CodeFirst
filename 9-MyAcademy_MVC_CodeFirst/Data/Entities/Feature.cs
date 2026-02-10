using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Feature
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Icon is required")]
        [MaxLength(100)]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
