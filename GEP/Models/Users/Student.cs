using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    [Table("estudante")]
    public class Student
    {
        public int Id { get; set; }

        public long Number { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
