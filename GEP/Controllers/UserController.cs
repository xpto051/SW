using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GEP.Data;
using GEP.Helpers;
using GEP.Models;
using GEP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper _mapper;

        public UserController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper, IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        //[Route("[getRoute]")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _userManager.Users.ToListAsync();
        }

        // GET: api/User/5  
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody]RegistrationViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.Role = "Admin";
            var userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, model.Role);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            if (model.Role == "Estudante")
            {
                await _context.Students.AddAsync(new Student { UserId = userIdentity.Id });
                await _context.SaveChangesAsync();
            }

            if(model.Role == "Admin")
            {
                await _context.Admins.AddAsync(new Admin { UserId = userIdentity.Id });
                await _context.SaveChangesAsync();
            }

            if (model.Role == "Docente")
            {
                await _context.Professors.AddAsync(new Professor { UserId = userIdentity.Id });
                await _context.SaveChangesAsync();
            }

            if (model.Role == "Coordenador")
            {
                await _context.Coordenators.AddAsync(new Coordenator { UserId = userIdentity.Id });
                await _context.SaveChangesAsync();
            }

            return new OkResult();
            /*
            var response = await _userManager.CreateAsync(user, "aA!12345678");
            
            if(response.Succeeded)
            {
                return Ok(user);
            } else
            {
                return Unauthorized(response.Errors);
            }*/
        }

        [HttpPost]
        [Route("CreateResp")]
        //POST : /api/User/CreateResp
        public async Task<ActionResult<User>> CreateResp([FromBody] RegistrationRespViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            model.Role = "ResponsavelEmpresa";
            User userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, model.Role);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            CompanyResp newResp = new CompanyResp()
            {
                User = userIdentity,
                Company = _context.Company.First(l => l.Id == model.CompanyId)
            };

            await _context.CompaniesResp.AddAsync(newResp);
            await _context.SaveChangesAsync();

            return Ok(newResp);
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/User/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                //get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim("UserID", user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType, role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JWT_SECRET)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            } 
            else
            {
                return BadRequest(new { message = "Email or password are incorrect." });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
