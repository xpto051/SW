using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("Projects")]
    public class Project : TFC
    {
        public string Theme { get; set; }

    }
}
