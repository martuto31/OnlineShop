using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Services.User
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailService(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var from = new EmailAddress("petrovmarti21@gmail.com", "Martin Petrov");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", message);
            await _sendGridClient.SendEmailAsync(msg);
        }
    }
}
