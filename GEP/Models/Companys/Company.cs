using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models

{
    [Table("Company")]
    public class Company
     
    {
        [Key]
        public int id { get; set; }
        public string  Sigla { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Descrição { get; set; }

        [Display(Name = "Empresa")]
        public string SiglaName { get { return CompanyName + "-" + Sigla; } }
    }
}
