using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.Notifications
{
    public class Notification
    {
        public Notification()
        {
            Creation = DateTime.Today;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This is required!")]
        [StringLength(2000, ErrorMessage = "This can´t exceed {1} characters!")]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public bool Seen { get; set; }

        public DateTime Creation { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public bool Sent { get; set; }

        public User User { get; set; }

        public bool isDeleted { get; set; }

    }
}
