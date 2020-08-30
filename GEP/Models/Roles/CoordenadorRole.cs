using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GEP.Models.Roles
{
    public class CoordenadorRole : IdentityRole
    {
        public CoordenadorRole() : base("Coordenador"){}
    }
}
