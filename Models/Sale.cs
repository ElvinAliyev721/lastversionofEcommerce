using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public virtual ICollection<SaleProduct> SaleProducts { get; set; }
    }
}
