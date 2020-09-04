using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("coordenator")]
    public class Coordenator
    {
        public int Id { get; set; }
        public long Number { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
