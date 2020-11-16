using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.Roles
{
    public class DocenteRole : IdentityRole
    {
        public DocenteRole() : base("Docente") { }
    }
}
