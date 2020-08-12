using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models
{
    public class User : IdentityUser
    {
        //[Required(ErrorMessage = "Erro isso é obrigatorio")]
        //[MaxLength(100)]
        //[Display(Name = "nome")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Utilizador")]
        public string Name { get { return FirstName + " " + LastName; } }
    }
}
