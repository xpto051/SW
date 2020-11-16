using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels
{
    public class CourseViewModel
    {
        [Required]
        public string Sigla { get; set; }
        [Required]
        public string Designacao { get; set; }
    }
}
