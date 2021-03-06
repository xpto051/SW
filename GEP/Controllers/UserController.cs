﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using GEP.Data;
using GEP.Helpers;
using GEP.Models;
using GEP.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailSender _emailSender;

        public UserController(ApplicationDbContext context, 
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
            var userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == userIdentity.Id);
            await _userManager.AddToRoleAsync(userIdentity, "Admin");

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _context.Admins.AddAsync(new Admin { UserId = userIdentity.Id });
            await _context.SaveChangesAsync();
            
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = Url.Action("ConfirmEmail", "User", new { userId = userIdentity.Id, code }, Request.Scheme);

            await _emailSender.SendEmailAsync(userIdentity.Email, "ConfirmarConta", $"Clique <a href={HtmlEncoder.Default.Encode(link)}>aqui</a> para confirmar a sua conta!");

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


        [HttpGet]
        [Route("confirm")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
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



        [HttpPost]
        [Route("Login")]
        //POST : /api/User/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user != null && await _userManager.CheckPasswordAsync(user, model.Password) && await _userManager.IsEmailConfirmedAsync(user))
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
            else if(user.EmailConfirmed == false)
            {
                return BadRequest(new { message = "O email ainda nao foi confirmado." });

            }
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
