using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GEP.Data;
using GEP.Models;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TFCsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TFCsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TFCs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TFC>>> GetTFCs()
        {
            return await _context.TFCs.ToListAsync();
        }

        // GET: api/TFCs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TFC>> GetTFC(int id)
        {
            var tFC = await _context.TFCs.FindAsync(id);

            if (tFC == null)
            {
                return NotFound();
            }

            return tFC;
        }

        // PUT: api/TFCs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTFC(int id, TFC tFC)
        {
            if (id != tFC.ID)
            {
                return BadRequest();
            }

            _context.Entry(tFC).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TFCExists(id))
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

        // POST: api/TFCs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TFC>> PostTFC(TFC tFC)
        {
            _context.TFCs.Add(tFC);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTFC", new { id = tFC.ID }, tFC);
        }

        // DELETE: api/TFCs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TFC>> DeleteTFC(int id)
        {
            var tFC = await _context.TFCs.FindAsync(id);
            if (tFC == null)
            {
                return NotFound();
            }

            _context.TFCs.Remove(tFC);
            await _context.SaveChangesAsync();

            return tFC;
        }

        private bool TFCExists(int id)
        {
            return _context.TFCs.Any(e => e.ID == id);
        }
    }
}
