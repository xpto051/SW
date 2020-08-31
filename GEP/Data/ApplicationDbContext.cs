using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GEP.Data
{
    public class ApplicationDbContext: IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<CompanyResp> CompaniesResp { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Coordenator> Coordenators{ get; set; }

        public DbSet<Company> Company { get; set; }
    }
}
