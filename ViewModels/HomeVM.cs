using Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class HomeVM
    {
        public ICollection<Category> Categories { get; set; }
        public ICollection<Slider> Sliders { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<GroupOfProduct> GroupOfProducts { get; set; }
    }
}
