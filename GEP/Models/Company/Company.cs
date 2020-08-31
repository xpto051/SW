using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.Company
{
    public class Company

    {
        public string Sigla { get; set; }
        public string CompanyName { get; set; }

        public string Descrição { get; set; }

        [Display(Name = "Empresa")]
        public string SiglaName { get { return CompanyName + "-" + Sigla; } }
    }
}