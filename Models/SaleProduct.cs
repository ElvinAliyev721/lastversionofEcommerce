using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class SaleProduct
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
