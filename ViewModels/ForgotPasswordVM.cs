using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required,EmailAddress,DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
