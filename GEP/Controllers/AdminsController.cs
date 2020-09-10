using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GEP.Data;
using GEP.Models;
using Microsoft.AspNetCore.Identity;
using GEP.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using GEP.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AdminsController(ApplicationDbContext context,
            UserManager<User> userManager,
            IMapper mapper,
            IOptions<ApplicationSettings> appSettings,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailSender = emailSender;
        }

        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            List<Admin> admins = await _context.Admins.ToListAsync();
            foreach (Admin s in admins)
            {
                s.User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == s.UserId);
            }
            return admins;
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            admin.User = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == admin.UserId);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        //GET: api/admins/myDetails
        [HttpGet]
        [Route("myDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<Object> GetMyDetails()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var admin = await _context.Admins.FirstAsync(c => c.UserId == user.Id);

            if (AdminExists(admin.Id))
            {
                return new
                {
                    admin.User.FirstName,
                    admin.User.LastName,
                    admin.User.PhoneNumber
                };
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/Admins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAdmin(int id, [FromBody] CompanyRespPutViewModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var admin = await _context.Admins.FirstAsync(c => c.UserId == user.Id);

            if ((model.NewPassword != null && model.ConfirmNewPassword == null) || model.NewPassword != model.ConfirmNewPassword)
            {
                return BadRequest("Please match the confirmNewpassword with newPassword");
            }

            if (model.Password != null)
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return BadRequest("Wrong password entered");
                }

            }

            if (model.PhoneNumber != null)
            {
                user.PhoneNumber = model.PhoneNumber;
            }

            if (model.FirstName != null)
            {
                user.FirstName = model.FirstName;
            }

            if (model.LastName != null)
            {
                user.LastName = model.LastName;
            }

            if (model.NewPassword != null && (model.NewPassword == model.ConfirmNewPassword))
            {
                await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            }

            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Admins
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostAdmin([FromBody] RegistrationAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User userIdentity = _mapper.Map<User>(model);
            userIdentity.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, "Admin");

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            

            Admin newAdmin = new Admin()
            {
                User = userIdentity
            };

            await _context.Admins.AddAsync(newAdmin);
            await _context.SaveChangesAsync();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = Url.Action("ConfirmEmail", "User", new { userId = userIdentity.Id, code }, Request.Scheme);

            await _emailSender.SendEmailAsync(userIdentity.Email, "ConfirmarConta", $"Clique <a href={HtmlEncoder.Default.Encode(link)}>aqui</a> para confirmar a sua conta! <br> A sua password é: 12345678jJ");


            return Ok(newAdmin);
        }

        [HttpGet]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User Não Existe!");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("Erro");
        }

        // DELETE: api/Admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Admin>> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == admin.UserId);

            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);

            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return admin;
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
