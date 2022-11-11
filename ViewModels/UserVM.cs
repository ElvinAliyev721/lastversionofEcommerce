using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool IsDeleted { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
    }
}
