using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GEP.Data;
using GEP.Models;
using GEP.ViewModels.TFCs;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternshipsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InternshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Internships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Internships>>> GetInternships()
        {
            return await _context.Internships.ToListAsync();
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

            CompanyResp cr = await _context.CompaniesResp.FindAsync(model.CompanyRespId);
            Company c = await _context.Company.FindAsync(cr.CompanyId);

            Internships i = new Internships()
            {
                Vagas = model.Vagas,
                Proposta = true,
                Aceite = false,
                Description = model.Description,
                Role = model.Role,               
                CompanyResp = cr, //await _context.CompaniesResp.FindAsync(model.CompanyRespId),
                Company = c,
                //CompanyId = c.Id

            };

            _context.Internships.Add(i);
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
