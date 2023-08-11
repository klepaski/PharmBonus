using Microsoft.IdentityModel.Tokens;
using System.Text;
using Med.DAL;
using System.Net.Mail;
using System.Net;

namespace Med.Services
{
    public interface IEmailService
    {
        public Task SendConfirmationEmail(int userId, string firstname, string lastname, string email);
    }

    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly ApplicationDbContext _db;
        private const string SENDER_EMAIL = "help.medgift@mail.ru";
        private const string PASSWORD = "NXqQyh2QWhkByQmBJqa5"; //Cvthnmyfgj7
        private const int PORT = 587;

        public EmailService(ILogger<EmailService> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task SendConfirmationEmail(int userId, string firstname, string lastname, string email)
        {
            MailAddress from = new MailAddress(SENDER_EMAIL, "MedGift");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Подтверждение почты MedGift";
            m.IsBodyHtml = true;
            m.Body = $"<h3>Привет, {firstname} {lastname}!</h3>" +
                    $"<p>Для подтверждения почты, пожалуйста, перейдите " +
                    $"по <a target=\"_self\" href=\"https://localhost:7006/api/Email?userId={userId}\">ссылке</a></p>" +
                    $"<br/><h4>Если вы не регистрировались в нашем приложении, обратитесь по телефону +375(33) 318-62-23.</h4>";
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", PORT);
            smtp.Credentials = new NetworkCredential(SENDER_EMAIL, PASSWORD);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            return;
        }
    }
}