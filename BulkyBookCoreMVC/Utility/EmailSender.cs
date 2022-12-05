using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;


namespace BulkyBookCoreMVC.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {



            try
            {
                SmtpClient smtp = new SmtpClient("consent.herosite.pro", 587);

                smtp.EnableSsl = false;
                smtp.Credentials = new NetworkCredential("info@jobinworkspace.in", "4+*TA!l74oXI");

                String from = "info@jobinworkspace.in";
                String to = email;
                
                
                MailMessage message = new MailMessage(from, to, subject, htmlMessage);
                message.IsBodyHtml = true;

                smtp.Send(message);

            }
            catch (Exception)
            {


            }

            return Task.CompletedTask;
        }
    }
}

/*
 * 
MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("samuel.jobin.jose@gmail.com");
                message.To.Add(new MailAddress(email));
                message.Subject = "email conformation";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlMessage;
                smtp.Port = 465;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("samuel.jobin.jose@gmail.com", "WA3O22gVfPLuZ");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
 * 
 * 
 * 
 * 
 * 
 * 
 * var emailtosend = new MimeMessage();

                emailtosend.From.Add(MailboxAddress.Parse("info@jobinworkspace.in"));
                emailtosend.To.Add(MailboxAddress.Parse(email));
                emailtosend.Subject = subject;
                emailtosend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text= htmlMessage };

                using (var emailClient=new SmtpClient())
                {
                    emailClient.Connect(  )
                }
 * */