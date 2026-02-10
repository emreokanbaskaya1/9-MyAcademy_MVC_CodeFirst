using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class TeamMember
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Position is required")]
        [MaxLength(100)]
        public string Position { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [MaxLength(200)]
        public string FacebookUrl { get; set; }

        [MaxLength(200)]
        public string TwitterUrl { get; set; }

        [MaxLength(200)]
        public string LinkedInUrl { get; set; }

        [MaxLength(200)]
        public string InstagramUrl { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
