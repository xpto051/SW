using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GEP.Data;
using GEP.Models.Notifications;
using GEP.Services;
using Microsoft.AspNetCore.Authorization;

namespace GEP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSender _emailSender = new EmailSender();

        public NotificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Notifications
        [HttpGet]
        [Authorize(Roles="Admin")] //só os admins é que podem ver todas as notificacoes de todos os users
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification()
        {
            return await _context.Notification.ToListAsync();
        }

        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _context.Notification.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            notification.Seen = true; //quando o user pedir a notificação passa a ser vista
            await _context.SaveChangesAsync();

            return notification;
        }

        // GET: api/Notifications/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationByUserId(int id)
        {
            var notifications = await _context.Notification.Where(n => n.UserId.Equals(id)).ToListAsync(); ;

            if (notifications == null)
            {
                //empty list
                return new List<Notification>();
            }

            foreach (Notification n in notifications) //nao tenho a certeza se precisas dessa informação ou nao
            {
                //user property in notification is filled with the right user
                n.User = await _context.Users.FindAsync(n.UserId);
            }

            return notifications;
        }

        /**
         * nao faz sentido alterares uma notificação
        // PUT: api/Notifications/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(int id, Notification notification)
        {
            if (id != notification.Id)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
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
        */

        // POST: api/Notifications
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification)
        {
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
            await _emailSender.SendEmailAsync(notification.UserId, "There is a new notification", notification.Message);

            return CreatedAtAction("GetNotification", new { id = notification.Id }, notification);
        }

        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Notification>> DeleteNotification(int id)
        {
            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.isDeleted = true;
            await _context.SaveChangesAsync();

            return notification;
        }

        private bool NotificationExists(int id)
        {
            return _context.Notification.Any(e => e.Id == id);
        }
    }
}
