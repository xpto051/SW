using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    
    public class Internships : TFC
    {
       
        [Required]
        public string Role { get; set; }
        [Required]
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
        [Required]
        
        public int CompanyRespId { get; set; }
        public virtual CompanyResp CompanyResp { get; set; }

    }
}
