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
using Microsoft.Extensions.Options;
using GEP.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public ProfessorsController(ApplicationDbContext context,
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

        // GET: api/Professors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessors()
        {
            List<Professor> professors = await _context.Professors.ToListAsync();
            foreach (Professor p in professors)
            {
                p.User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == p.UserId);
            }
            return professors;
        }

        // GET: api/Professors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Professor>> GetProfessor(int id)
        {
            var professor = await _context.Professors.FindAsync(id);
            professor.User = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == professor.UserId);

            if (professor == null)
            {
                return NotFound();
            }

            return professor;

        }

        //GET: api/Professors/myDetails
        [HttpGet]
        [Route("myDetails")]
        [Authorize(Roles = "Docente")]
        public async Task<Object> GetMyDetails()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var prof = await _context.Professors.FirstAsync(c => c.UserId == user.Id);

            if (ProfessorExists(prof.Id))
            {
                return new
                {
                    prof.User.FirstName,
                    prof.User.LastName,
                    prof.User.PhoneNumber
                };
            }
            else
            {
                return BadRequest();
            }
        }

        // POST: api/Professors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Professor>> PostProfessor([FromBody] RegistrationProfViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, "Docente");

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            Professor newProf = new Professor()
            {
                User = userIdentity,
                Number = model.Number
            };

            await _context.Professors.AddAsync(newProf);
            await _context.SaveChangesAsync();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = Url.Action("ConfirmEmail", "User", new { userId = userIdentity.Id, code }, Request.Scheme);

            await _emailSender.SendEmailAsync(userIdentity.Email, "ConfirmarConta", $"Clique <a href={HtmlEncoder.Default.Encode(link)}>aqui</a> para confirmar a sua conta! <br> A sua password é: 12345678jJ");


            return Ok(newProf);
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

        // PUT: api/Professors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "Docente")]
        public async Task<IActionResult> PutDocente(int id, [FromBody] CompanyRespPutViewModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var prof = await _context.Professors.FirstAsync(c => c.UserId == user.Id);

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
                if (!ProfessorExists(id))
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

        // DELETE: api/Professors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Professor>> DeleteProfessor(int id)
        {
            var prof = await _context.Professors.FindAsync(id);
            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == prof.UserId);

            if (prof == null)
            {
                return NotFound();
            }

            _context.Professors.Remove(prof);

            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return prof;
        }

        private bool ProfessorExists(int id)
        {
            return _context.Professors.Any(e => e.Id == id);
        }
    }
}
