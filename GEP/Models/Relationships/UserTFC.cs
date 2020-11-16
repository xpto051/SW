using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GEP.Models;

namespace GEP.Models
{
    public class UserTFC
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("TFC")]
        public int TFCId { get; set; }
        public TFC TFC { get; set; }
        [ForeignKey("Professor")]
        public string ProfessorId { get; set; }
        public User Professor { get; set; }
        public bool wasAccepted { get; set; }
        public bool isApplication { get; set; }
    }
}
