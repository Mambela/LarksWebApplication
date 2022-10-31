using LarksWebApp.Services.Loggers;
using System.Net;
using System.Net.Mail;

namespace LarksWebApp.Services.Mailer
{
    public class EmailHelper
    {
        public readonly ILoggerTrace serviceLogger;

        public EmailHelper() 
        {
            
        }
        public bool SendEmail(string userEmail,string body) {

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("jacklumbanzamo@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));


            mailMessage.Subject = "Larks Account confirmation";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;    



            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("securesally@gmail.com", "zbjgkqprucevuvta");
            client.Host = "smtpout.secureserver.net";
            client.Port = 80;

            try
            {
                client.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                // serviceLogger.LogError("Error occurred while trying to send mail -> " + ex.Message);

                ServiceLogger serviceLogger = new ServiceLogger();
                serviceLogger.LogError("Error occurred while trying to send mail -> " + ex.Message);
                return false;
            }
        }

        public bool SendEmailMailTrapAsync(string userEmail, string body) {

            try
            {

                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("fbc8f7dbb87a2b", "c0fb9241b03c87"),
                    EnableSsl = true
                };

                client.Send("non-reply@example.com", userEmail, "Account Verification", body);

                return true;
            }
            catch (Exception ex)
            {
                ServiceLogger serviceLogger = new ServiceLogger();
                serviceLogger.LogError("Error while attempting to send email with mail " + ex.Message);
                return false;
            }

        }
    }
}
