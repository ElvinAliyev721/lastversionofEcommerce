using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public string Image { get; set; }
        public int GroupId { get; set; }
        public GroupOfProduct Group { get; set; }
    }
}
