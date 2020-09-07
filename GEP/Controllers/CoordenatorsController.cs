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
    public class CoordenatorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public CoordenatorsController(ApplicationDbContext context,
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

        // GET: api/Coordenators
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coordenator>>> GetCoordenators()
        {
            List<Coordenator> coordenators = await _context.Coordenators.ToListAsync();
            foreach (Coordenator s in coordenators)
            {
                s.User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == s.UserId);
                s.Course = await _context.Course.FindAsync(s.CourseId);
            }
            return coordenators;
        }

        // GET: api/Coordenators/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coordenator>> GetCoordenator(int id)
        {
            var coordenator = await _context.Coordenators.FindAsync(id);
            coordenator.User = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == coordenator.UserId);
            coordenator.Course = await _context.Course.FindAsync(coordenator.CourseId);

            if (coordenator == null)
            {
                return NotFound();
            }

            return coordenator;
        }


        // PUT: api/Coordenators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Authorize(Roles = "Coordenador")]
        public async Task<IActionResult> PutEstudante(int id, [FromBody] CompanyRespPutViewModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var coord = await _context.Coordenators.FirstAsync(c => c.UserId == user.Id);

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
                if (!CoordenatorExists(id))
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

        // POST: api/Coordenators
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Coordenator>> PostCoordenator([FromBody] RegistrationCoordenatorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, "Coordenador");

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));


            Coordenator newCoordenator = new Coordenator()
            {
                User = userIdentity,
                Number = model.Number,
                Course = _context.Course.First(c => c.Id == model.CourseId)
            };

            await _context.Coordenators.AddAsync(newCoordenator);
            await _context.SaveChangesAsync();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = Url.Action("ConfirmEmail", "User", new { userId = userIdentity.Id, code }, Request.Scheme);

            await _emailSender.SendEmailAsync(userIdentity.Email, "ConfirmarConta", $"Clique <a href={HtmlEncoder.Default.Encode(link)}>aqui</a> para confirmar a sua conta! <br> A sua password é: 12345678jJ");


            return Ok(newCoordenator);
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

        //GET: api/Coordenators/myDetails
        [HttpGet]
        [Route("myDetails")]
        [Authorize(Roles = "Coordenador")]
        public async Task<Object> GetMyDetails()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var coord = await _context.Coordenators.FirstAsync(c => c.UserId == user.Id);

            if (CoordenatorExists(coord.Id))
            {
                return new
                {
                    coord.User.FirstName,
                    coord.User.LastName,
                    coord.User.PhoneNumber
                };
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Coordenators/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coordenator>> DeleteCoordenator(int id)
        {
            var coordenator = await _context.Coordenators.FindAsync(id);
            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == coordenator.UserId);

            if (coordenator == null)
            {
                return NotFound();
            }

            _context.Coordenators.Remove(coordenator);

            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return coordenator;
        }

        private bool CoordenatorExists(int id)
        {
            return _context.Coordenators.Any(e => e.Id == id);
        }
    }
}
