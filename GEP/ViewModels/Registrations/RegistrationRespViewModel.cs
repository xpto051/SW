using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels
{
    public class RegistrationRespViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Empresa")]
        public int CompanyId { get; set; }
    }
}
