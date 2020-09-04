﻿using System;
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
using Microsoft.AspNetCore.Identity.UI.Services;
using AutoMapper;
using Microsoft.Extensions.Options;
using GEP.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyRespsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public CompanyRespsController(ApplicationDbContext context,
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

        // GET: api/CompanyResps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyResp>>> GetCompaniesResp()
        {
            List<CompanyResp> companyResps = await _context.CompaniesResp.ToListAsync();
            foreach (CompanyResp s in companyResps)
            {
                s.User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == s.UserId);
                s.Company = await _context.Company.FindAsync(s.CompanyId);
            }
            return companyResps;
        }

        // GET: api/CompanyResps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyResp>> GetCompanyResp(int id)
        {
            var companyResp = await _context.CompaniesResp.FindAsync(id);
            companyResp.User = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == companyResp.UserId);
            companyResp.Company = await _context.Company.FindAsync(companyResp.CompanyId);
            if (companyResp == null)
            {
                return NotFound();
            }

            return companyResp;
        }

        [HttpPost]
        //POST : /api/CompanyResps
        public async Task<ActionResult<User>> PostCompanyResp([FromBody] RegistrationRespViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User userIdentity = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userIdentity, "12345678jJ");
            await _userManager.AddToRoleAsync(userIdentity, "ResponsavelEmpresa");

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            CompanyResp newResp = new CompanyResp()
            {
                User = userIdentity,
                Company = _context.Company.First(l => l.Id == model.CompanyId)
            };

            await _context.CompaniesResp.AddAsync(newResp);
            await _context.SaveChangesAsync();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = Url.Action("ConfirmEmail", "User", new { userId = userIdentity.Id, code }, Request.Scheme);

            await _emailSender.SendEmailAsync(userIdentity.Email, "ConfirmarConta", $"Clique <a href={HtmlEncoder.Default.Encode(link)}>aqui</a> para confirmar a sua conta!");


            return Ok(newResp);
        }

        // PUT: api/CompanyResps/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyResp(int id, CompanyResp companyResp)
        {
            if (id != companyResp.Id)
            {
                return BadRequest();
            }

            _context.Entry(companyResp).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyRespExists(id))
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

        // DELETE: api/CompanyResps/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CompanyResp>> DeleteCompanyResp(int id)
        {
            var companyResp = await _context.CompaniesResp.FindAsync(id);
            var user = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == companyResp.UserId);

            if (companyResp == null)
            {
                return NotFound();
            }

            _context.CompaniesResp.Remove(companyResp);

            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();

            return companyResp;
        }

        private bool CompanyRespExists(int id)
        {
            return _context.CompaniesResp.Any(e => e.Id == id);
        }
    }
}