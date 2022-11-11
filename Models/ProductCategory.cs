using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
