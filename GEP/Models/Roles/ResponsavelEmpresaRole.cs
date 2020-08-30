using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.Roles
{
    public class ResponsavelEmpresaRole : IdentityRole
    {
        public ResponsavelEmpresaRole() : base("ResponsavelEmpresa") { }
    }
}
