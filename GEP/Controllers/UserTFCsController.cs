using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GEP.Data;
using GEP.Models;
using GEP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTFCsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserTFCsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/UserTFCs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTFC>>> GetUserTFC()
        {
            return await _context.UserTFC.ToListAsync();
        }

        // GET: api/UserTFCs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTFC>> GetUserTFC(int id)
        {
            var userTFC = await _context.UserTFC.FindAsync(id);

            if (userTFC == null)
            {
                return NotFound();
            }

            userTFC.User = await _context.Users.FindAsync(userTFC.UserId);
            userTFC.TFC = await _context.TFCs.FindAsync(userTFC.TFCId);
            if(userTFC.ProfessorId != null)
            {
                userTFC.Professor = await _context.Users.FindAsync(userTFC.ProfessorId);
            }

            return userTFC;
        }

        // GET: api/UserTFCs/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserTFC>>> GetUserTFCByUserId(int id)
        {
            var userTFC = await _context.UserTFC.Where(n => n.UserId.Equals(id)).ToListAsync(); ;

            if (userTFC == null)
            {
                return NotFound();
            }

            foreach(UserTFC ut in userTFC)
            {
                ut.User = await _context.Users.FindAsync(ut.UserId);
                ut.TFC = await _context.TFCs.FindAsync(ut.TFCId);
                if (ut.ProfessorId != null)
                {
                    ut.Professor = await _context.Users.FindAsync(ut.ProfessorId);
                }
            }

            return userTFC;
        }

        // PUT: api/UserTFCs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTFC(int id, UserTFC userTFC)
        {
            if (id != userTFC.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTFC).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTFCExists(id))
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

        // PUT: api/UserTFCs/accept/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Route("accept")]
        public async Task<IActionResult> AcceptUserTFC(int id)
        {
            var userTFC = await _context.UserTFC.FindAsync(id);
            if(userTFC == null)
            {
                return NotFound();
            }

            userTFC.wasAccepted = true;
            userTFC.isApplication = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTFCExists(id))
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

        // PUT: api/UserTFCs/reject/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("reject/{id}")]
        public async Task<IActionResult> RejectUserTFC(int id)
        {
            var userTFC = await _context.UserTFC.FindAsync(id);
            if (userTFC == null)
            {
                return NotFound();
            }

            userTFC.wasAccepted = false;
            userTFC.isApplication = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTFCExists(id))
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

        // POST: api/UserTFCs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles ="Estudante")]
        public async Task<ActionResult<UserTFC>> PostUserTFC(UserTFCViewModel userTFC)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var est = await _context.Students.FirstAsync(c => c.UserId == user.Id);

            var prof = await _context.Professors.FindAsync(userTFC.ProfessorId);
            var profUser = await _userManager.FindByIdAsync(prof.UserId);

            UserTFC c = new UserTFC()
            {
                User = user,
                TFC = await _context.TFCs.FindAsync(userTFC.TFCId),
                wasAccepted = false,
                Professor = profUser,
                isApplication = true
            };

            _context.UserTFC.Add(c);
            await _context.SaveChangesAsync();

            return Ok(c);
        }

        // DELETE: api/UserTFCs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserTFC>> DeleteUserTFC(int id)
        {
            var userTFC = await _context.UserTFC.FindAsync(id);
            if (userTFC == null)
            {
                return NotFound();
            }

            _context.UserTFC.Remove(userTFC);
            await _context.SaveChangesAsync();

            return userTFC;
        }

        private bool UserTFCExists(int id)
        {
            return _context.UserTFC.Any(e => e.Id == id);
        }
    }
}
