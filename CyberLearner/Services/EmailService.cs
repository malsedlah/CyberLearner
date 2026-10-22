using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace CyberLearner.Services
{
    public class EmailService
    {
        public void SendVerificationCode(string toEmail, string fullName, string code)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            string user = ConfigurationManager.AppSettings["SmtpUser"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string fromName = ConfigurationManager.AppSettings["SmtpFromName"];
            string minutes = ConfigurationManager.AppSettings["OtpMinutes"];

            MailMessage message = new MailMessage();
            message.From = new MailAddress(user, fromName);
            message.To.Add(toEmail);
            message.Subject = "Your CyberLearner verification code";
            message.Body = "Hello " + fullName + ",\n\n" +
                           "Your CyberLearner verification code is: " + code + "\n\n" +
                           "This code expires in " + minutes + " minutes.\n" +
                           "If you did not register, please ignore this email.\n\n" +
                           "CyberLearner Team";

            SmtpClient client = new SmtpClient(host, port);
            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);
            client.Credentials = new NetworkCredential(user, password);
            client.Send(message);
        }
    }
}