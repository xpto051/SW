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
using Microsoft.AspNetCore.Authorization;
using GEP.ViewModels.TFCs;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ProjectsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            List<Project> projectList = await _context.TFCs.OfType<Project>().ToListAsync();
            foreach (Project p in projectList)
            {
                p.Professor = await _context.Professors.FirstOrDefaultAsync(u => u.Id == p.ProfessorId);
            }
            return projectList;
        }

        [HttpGet]
        [Route("proposeProjects")]
        // GET: api/Projects/proposeProjects
        public async Task<ActionResult<IEnumerable<Project>>> GetProposeProjects()
        {
            List<Project> projectList = await _context.TFCs.OfType<Project>().Where(c => c.Proposta == true).ToListAsync();
            foreach (Project p in projectList)
            {
                p.Professor = await _context.Professors.FirstOrDefaultAsync(u => u.Id == p.ProfessorId);
            }
            return projectList;
        }

        [HttpPut]
        [Route("acceptPropose/{id}")]
        // PUT: api/Projects/acceptPropose/1
        public async Task<IActionResult> PutAcceptPropose(int id)
        {

            Project i = await _context.TFCs.OfType<Project>().FirstOrDefaultAsync(i => i.ID == id);
            if (i.Proposta == false)
            {
                return BadRequest("This project is not a proposed project so u cant accept it");
            }
            i.Aceite = true;
            i.Proposta = false;

            _context.Update(i);
            await _context.SaveChangesAsync();

            return Ok(i);
        }

        [HttpPut]
        [Route("rejectPropose/{id}")]
        // PUT: api/Projects/rejectPropose/1
        public async Task<IActionResult> PutRejectPropose(int id)
        {

            Project i = await _context.TFCs.OfType<Project>().FirstOrDefaultAsync(i => i.ID == id);
            if (i.Proposta == false)
            {
                return BadRequest("This internship is not a proposed internship so u cant accept it");
            }
            i.Aceite = false;
            i.Proposta = false;

            _context.Update(i);
            await _context.SaveChangesAsync();

            return Ok(i);
        }

        [HttpGet]
        [Route("availableProjects")]
        // GET: api/Projects/availableProjects
        public async Task<ActionResult<IEnumerable<Project>>> GetAvailablePRojects()
        {
            List<Project> projectList = await _context.TFCs.OfType<Project>().Where(c => c.Proposta == false && c.Aceite == true).ToListAsync();
            foreach (Project p in projectList)
            {
                p.Professor = await _context.Professors.FirstOrDefaultAsync(u => u.Id == p.ProfessorId);
            }
            return projectList;
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }



        // PUT: api/Projects/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ID)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects/proposeProject
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("proposeProject")]
        public async Task<ActionResult<Project>> ProposeProject(ProjectModel model)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var doc = await _context.Professors.FirstAsync(c => c.UserId == user.Id);

            Project newProject = new Project()
            {
                Description = model.Description,
                Theme = model.Theme,
                Professor = doc,
                Vagas = model.Vagas,
                Aceite = false,
                Proposta = true
            };

            await _context.TFCs.AddAsync(newProject);
            await _context.SaveChangesAsync();
            return Ok(newProject);

        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return project;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.ID == id);
        }
    }
}
