using System;
using System.ComponentModel.DataAnnotations;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Blog
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [MaxLength(500)]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [MaxLength(100)]
        public string Author { get; set; }

        [MaxLength(100)]
        public string CategoryName { get; set; }

        public int CommentCount { get; set; } = 0;

        public DateTime PublishDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
    }
}
