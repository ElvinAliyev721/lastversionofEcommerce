using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, MinLength(100)]
        public string Message { get; set; }
        public string SendedTime { get; set; }
    }
}
