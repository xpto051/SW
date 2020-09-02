using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GEP.Data;
using Microsoft.AspNetCore.Identity;
using GEP.Models;
using AutoMapper;
using System;
using GEP.ViewModels.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GEP.Helpers;
using GEP.Models.Roles;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using GEP.Services;

namespace GEP
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Inject appsettings
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.User.AllowedUserNameCharacters = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                options.User.RequireUniqueEmail = true;
            }).AddRoleManager<RoleManager<IdentityRole>>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAutoMapper(typeof(ViewModelToEntityMappingProfile));

            services.AddTransient<IEmailSender, EmailSender>();

            //Jwt authentication
            var secretKey = Encoding.ASCII.GetBytes(Configuration["ApplicationSettings:JWT_SECRET"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            /*
             * app.UseRouting();
             * 
            app.UseEndpoints(endpoints =>
            { 
             endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller}/{action=Index}/{id?}");

            }); */

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            CreateRoles(serviceProvider).Wait();
        }

        /// <summary>
        /// Cria os roles caso não existam
        /// </summary>
        /// <param name="roleManager"></param>
        public async static Task CreateRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole adminRole = new AdminRole();
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("Coordenador"))
            {
                IdentityRole coordenadorRole = new CoordenadorRole();
                await roleManager.CreateAsync(coordenadorRole);
            }
            if (!await roleManager.RoleExistsAsync("Docente"))
            {
                IdentityRole docenteRole = new DocenteRole();
                await roleManager.CreateAsync(docenteRole);
            }
            if (!await roleManager.RoleExistsAsync("Estudante"))
            {
                IdentityRole estudanteRole = new EstudanteRole();
                await roleManager.CreateAsync(estudanteRole);
            }

            if (!await roleManager.RoleExistsAsync("ResponsavelEmpresa"))
            {
                IdentityRole responsavelEmpresaRole = new ResponsavelEmpresaRole();
                await roleManager.CreateAsync(responsavelEmpresaRole);
            }
        }
    }
}
