using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string Sigla { get; set; }
        public string Designação { get; set; }
    }
}
