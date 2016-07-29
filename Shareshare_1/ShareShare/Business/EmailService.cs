using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace shareshare.Business
{
    public class EmailService
    {
        SmtpClient _smtp = new SmtpClient("email-smtp.us-east-1.amazonaws.com", 587);
        string _restfulUrl = Infrastructure.Util.ReadSetting(Infrastructure.Util.KEY_RESTFULURL);
        public EmailService()
        {
            _smtp.Credentials = new NetworkCredential("AKIAIEMEZC7V72KDJGDQ", "Al9rHbRr7fQ0785F5MzqFfg4qcycZ6R6q+ohY6zamYfw");
            _smtp.EnableSsl = true;
        }



        public bool SendEmail(string email, string token)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("xxhu83@gmail.com", "share share ltd");
                    mail.To.Add(email);
                    mail.Subject = "share share account verification";
                    string body = string.Format("Click to activate your account : {0}activation/{1}",_restfulUrl, token);
                    mail.Body = body;
                    mail.IsBodyHtml = true;
                    _smtp.Send(mail);

                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }


    }

}
