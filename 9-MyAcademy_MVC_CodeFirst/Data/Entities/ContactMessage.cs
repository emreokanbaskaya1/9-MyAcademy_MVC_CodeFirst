using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(50)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Insurance type is required")]
        [MaxLength(100)]
        public string InsuranceType { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [MaxLength(200)]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required")]
        public string Message { get; set; }

        public System.DateTime CreatedDate { get; set; } = System.DateTime.Now;

        public bool IsRead { get; set; } = false;

        // AI Classification Fields (Hugging Face)
        [MaxLength(50)]
        public string AICategory { get; set; }

        public double? AIConfidence { get; set; }

        public bool AIIsUrgent { get; set; } = false;
    }
}
