using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.ViewModels
{
    public class RegistrationStudentsViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Número do Estudante")]
        public long Number { get; set; }

        [Required]
        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int CourseId { get; set; }
    }
}
