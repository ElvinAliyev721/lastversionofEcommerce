using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSame { get; set; }
        public int GroupOfProductId { get; set; }
        public virtual GroupOfProduct GroupOfProduct { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
