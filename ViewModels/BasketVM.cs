using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class BasketVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public int BasketCount { get; set; }
    }
}
