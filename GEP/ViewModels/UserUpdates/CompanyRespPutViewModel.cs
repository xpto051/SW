using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels
{
    public class CompanyRespPutViewModel
    {
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }

        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }

        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }

    }
}
