using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class Size
    {
        public int Id { get; set; }
        [Required]
        public string Measure { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
