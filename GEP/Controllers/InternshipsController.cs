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
using GEP.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public InternshipsController(ApplicationDbContext context,UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Internships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Internships>>> GetInternships()
        {
            List<Internships> internShipList =  await _context.TFCs.OfType<Internships>().ToListAsync();
            foreach (Internships i in internShipList)
            {
                i.CompanyResp = await _context.CompaniesResp.FirstOrDefaultAsync(u => u.Id == i.CompanyRespId);
                i.Company = await _context.Company.FirstOrDefaultAsync(c => c.Id == i.CompanyId);
            }
            return internShipList;
        }

        // GET: api/Internships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Internships>> GetInternships(int id)
        {
            var internships = await _context.Internships.FindAsync(id);

            if (internships == null)
            {
                return NotFound();
            }

            return internships;
        }

        // PUT: api/Internships/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInternships(int id, Internships internships)
        {
            if (id != internships.ID)
            {
                return BadRequest();
            }

            _context.Entry(internships).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InternshipsExists(id))
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

        
        [HttpPost]
        [Authorize(Roles = "Admin,ResponsavelEmpresa")]
        [Route("proposeInternship")]
        //POST : /api/Internships/proposeInternship
        public async Task<ActionResult<Internships>> ProposeInternship(IntershipModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var resp = await _context.CompaniesResp.FirstAsync(c => c.UserId == user.Id);

            Company comp = await _context.Company.FindAsync(resp.CompanyId);

            Internships newInternship = new Internships()
            {
                Description = model.Description,
                Role = model.Role,
                Vagas = model.Vagas,
                CompanyResp = resp,
                Company = comp,
                Aceite = false,
                Proposta = true
            };

            await _context.TFCs.AddAsync(newInternship);
            await _context.SaveChangesAsync();
            return Ok(newInternship);

        } 

        // POST: api/Internships
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Internships>> PostInternships(IntershipModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Internships i = new Internships()
            {
                Vagas = model.Vagas,
                Proposta = true,
                Aceite = false,
                Description = model.Description,
                Role = model.Role,   
                //CompanyId = c.Id

            };

            _context.TFCs.Add(i);
            await _context.SaveChangesAsync();

            return Ok( i);
        }

        // DELETE: api/Internships/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Internships>> DeleteInternships(int id)
        {
            var internships = await _context.Internships.FindAsync(id);
            if (internships == null)
            {
                return NotFound();
            }

            _context.Internships.Remove(internships);
            await _context.SaveChangesAsync();

            return internships;
        }

        private bool InternshipsExists(int id)
        {
            return _context.Internships.Any(e => e.ID == id);
        }
    }
}
