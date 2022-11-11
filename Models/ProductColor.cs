using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class ProductColor
    {
        public int Id { get; set; }
        [Required]
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
