using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GEP.Data;
using GEP.Helpers;
using GEP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly object _userManager;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
          
        }

        [HttpGet]
        //GET : api/Company
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanyList()
        {
            return await _context.Company.ToListAsync();
        }

        [HttpGet("{CompanyName}")]
        //GET :api/Company/Name
        public async Task<ActionResult<Company>> ShowCompanyDetails(string CompanyName)
        {
            Company model = await _context.Company.FindAsync(CompanyName);
            if (CompanyName == null)
            {
                return NotFound();
            }
            return model;
             
        }

        [HttpPut]
        //PUT : api/Company/Name
        public async Task<IActionResult> PutCompanyDetails(string companyName, Company company)
        {
            if (companyName != company.CompanyName)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }
            return Ok();
        }




    }
}
