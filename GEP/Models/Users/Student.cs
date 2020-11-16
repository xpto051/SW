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

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public bool isEnrolled { get; set; }
    }
}