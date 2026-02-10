using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class About
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Subtitle is required")]
        [MaxLength(200)]
        public string Subtitle { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        public int InsurancePolicies { get; set; } = 0;

        public int AwardsWon { get; set; } = 0;

        public int SkilledAgents { get; set; } = 0;

        public int TeamMembers { get; set; } = 0;

        public bool IsActive { get; set; } = true;
    }
}
