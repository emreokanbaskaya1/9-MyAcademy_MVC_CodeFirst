using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}