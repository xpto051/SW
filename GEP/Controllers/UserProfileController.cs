 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        // get: /api/userprofile
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
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