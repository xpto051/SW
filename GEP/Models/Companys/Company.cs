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
        public int Id { get; set; }
        public string  Sigla { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name = "Site")]
        public string URL { get; set; }
    }
}
