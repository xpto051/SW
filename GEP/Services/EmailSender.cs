using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GEP.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080")
            };
            await client.PostAsJsonAsync("/send", new { mail = email, subject, html = htmlMessage });
        }
    }

}
