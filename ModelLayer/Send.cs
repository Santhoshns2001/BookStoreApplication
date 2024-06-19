using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class Send
    {
        public string SendMail(string ToEmail, string Token)
        {
            string FromMail = "n.s.santhosh116@gmail.com";
            MailMessage Message = new MailMessage(FromMail, ToEmail);
            string MailBody = "the token for reset password : " + Token;
            Message.Subject = "Tokwn Generated for resetting Password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("n.s.santhosh116@gmail.com", "xtho vveq yjac uwap");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(Message);
            return ToEmail;

        }
    }
}
