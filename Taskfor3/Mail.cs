using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Taskfor3
{
    internal class Mail
    {
        public static async Task SendEmailAsync(string massage)
        {
            MailAddress from = new MailAddress("vtostakan@gmail.com", "Salamaleikum");
            MailAddress to = new MailAddress("Kerimkhan2021@mail.ru");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "ModeTest";
            m.Body = $"{massage}";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("vtostakan@gmail.com", "damnsowsow45");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Send Letter");
        }
    }
}
