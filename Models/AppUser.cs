using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class AppUser:IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
