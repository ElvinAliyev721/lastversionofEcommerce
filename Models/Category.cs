using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required, MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public bool IsMain { get; set; }
        public int? ParentId { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> Children { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
