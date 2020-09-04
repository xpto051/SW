 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEP.Data;
using GEP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserProfileController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        // get: /api/userprofile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var UserLoginInfo = await _context.Admins.FirstOrDefaultAsync(u => u.UserId == userId);
                return new
                {
                    UserLoginInfo.User.Name,
                    UserLoginInfo.User.Email,
                };
            }

            if (await _userManager.IsInRoleAsync(user, "Estudante"))
            {
                var UserLoginInfo = await _context.Students.FirstOrDefaultAsync(u => u.UserId == userId);
                return new
                {
                    UserLoginInfo.User.Name,
                    UserLoginInfo.User.Email,
                    UserLoginInfo.Number
                };
            }

            if (await _userManager.IsInRoleAsync(user, "Docente"))
            {
                var UserLoginInfo = await _context.Professors.FirstOrDefaultAsync(u => u.UserId == userId);
                return new
                {
                    UserLoginInfo.User.Name,
                    UserLoginInfo.User.Email,
                    UserLoginInfo.Number
                };
            }
            if (await _userManager.IsInRoleAsync(user, "Coordenador"))
            {
                var UserLoginInfo = await _context.Coordenators.FirstOrDefaultAsync(u => u.UserId == userId);
                return new
                {
                    UserLoginInfo.User.Name,
                    UserLoginInfo.User.Email,
                    UserLoginInfo.Number
                };
            }

            if (await _userManager.IsInRoleAsync(user, "ResponsavelEmpresa"))
            {
                var UserLoginInfo = await _context.CompaniesResp.FirstOrDefaultAsync(u => u.UserId == userId);
                var company = await _context.Company.FirstOrDefaultAsync(c => c.Id == UserLoginInfo.CompanyId);
                return new
                {
                    UserLoginInfo.User.Name,
                    UserLoginInfo.User.Email,
                    UserLoginInfo.Company
                };
            }

            return new
            {
                user.Name,
                user.Email
            };
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Estudante")]
        [Route("forAdmin")]

        public string getForAdmin()
        {
            return "metodo do admin teste";
        }
    }
}