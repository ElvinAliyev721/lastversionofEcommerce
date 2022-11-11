using Code.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.ViewModels
{
    public class DashboardVM
    {
        public ICollection<Contact> Contacts { get; set; }
    }
}
