using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels
{
    public class RegistrationAdminViewModel
    {
        [Required]
        public string Email { get; set; }
    }
}
