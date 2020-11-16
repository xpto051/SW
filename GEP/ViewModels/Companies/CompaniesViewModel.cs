using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels.Companies
{
    public class CompaniesViewModel
    {
        public string Sigla { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Site")]
        public string URL { get; set; }
    }
}
