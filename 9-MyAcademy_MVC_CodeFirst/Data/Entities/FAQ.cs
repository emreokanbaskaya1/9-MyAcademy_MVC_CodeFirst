using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class FAQ
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question is required")]
        [MaxLength(500)]
        public string Question { get; set; }

        [Required(ErrorMessage = "Answer is required")]
        public string Answer { get; set; }

        public int OrderNumber { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
