using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GEP.Models.Notifications;
using GEP.Models.File;

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
        public DbSet<Internships> Internships { get; set; }
        public DbSet<TFC> TFCs { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<GEP.Models.Notifications.Notification> Notification { get; set; }
        public DbSet<GEP.Models.File.File> File { get; set; }
        public DbSet<GEP.Models.UserTFC> UserTFC { get; set; }
        public DbSet<GEP.Models.Project> Project { get; set; }
    }
}
