using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class SaleVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public double Total { get; set; }
        public ICollection<SaleProductVM> SaleProductVMs { get; set; }
    }
}
