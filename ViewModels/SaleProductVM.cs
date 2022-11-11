using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class SaleProductVM
    {
        public int Id { get; set; }
        public double Price { get; set; }
        //public string Size { get; set; }
        //public string Color { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
    }
}
