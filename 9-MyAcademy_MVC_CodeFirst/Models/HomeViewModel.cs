using _9_MyAcademy_MVC_CodeFirst.Data.Entities;
using System.Collections.Generic;

namespace _9_MyAcademy_MVC_CodeFirst.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Feature> Features { get; set; }
        public About About { get; set; }
        public List<Product> Services { get; set; }
        public List<FAQ> FAQs { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public Contact ContactInfo { get; set; }
    }
}
