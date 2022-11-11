using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class GroupOfProduct
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        [Required]
        public IFormFile[] Photos { get; set; }
        public int Count { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
